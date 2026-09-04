CREATE OR ALTER PROCEDURE sp_GetContractOverdueSummary
    @ContractNo NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @cols NVARCHAR(MAX);
    DECLARE @colsCast NVARCHAR(MAX);
    DECLARE @totalExpr NVARCHAR(MAX);
    DECLARE @sql NVARCHAR(MAX);

    -- สร้าง column list สำหรับ PIVOT (ใช้ใน FOR ... IN)
    SELECT @cols = STRING_AGG(QUOTENAME(EnumCode), ',')
    FROM SysEnum
    WHERE EnumName = 'OverdueType';

    -- สร้าง column list แบบ CAST เป็น DECIMAL สำหรับ SELECT
    SELECT @colsCast = STRING_AGG('CAST(ISNULL(' + QUOTENAME(EnumCode) + ', 0) AS DECIMAL(18,2)) AS ' + QUOTENAME(EnumCode), ',')
    FROM SysEnum
    WHERE EnumName = 'OverdueType';

    -- สร้าง expression สำหรับ Total
    SELECT @totalExpr = STRING_AGG('ISNULL(' + QUOTENAME(EnumCode) + ',0)', ' + ')
    FROM SysEnum
    WHERE EnumName = 'OverdueType';

    SET @sql = '
    SELECT 
        OverdueTermNo,
        DueDate,
        ' + @colsCast + ',
        CAST(' + @totalExpr + ' AS DECIMAL(18,2)) AS Total
    FROM
    (
        SELECT 
            co.OverdueTermNo,
            co.DueDate,
            se.EnumCode,
            co.OverdueAmount
        FROM ContractOverdue co
        LEFT JOIN SysEnum se
            ON co.OverdueType = se.EnumCode
        WHERE (se.EnumName = ''OverdueType'' OR se.EnumName IS NULL)
          AND co.ContractNo = @ContractNo
    ) src
    PIVOT
    (
        SUM(OverdueAmount)
        FOR EnumCode IN (' + @cols + ')
    ) p
    ORDER BY OverdueTermNo';

    EXEC sp_executesql 
        @sql,
        N'@ContractNo NVARCHAR(20)',
        @ContractNo;
END