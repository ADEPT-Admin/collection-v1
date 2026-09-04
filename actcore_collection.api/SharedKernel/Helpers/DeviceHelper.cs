using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace SharedKernel.Helpers
{
    /// <summary>
    /// Utility สำหรับวิเคราะห์ข้อมูลอุปกรณ์จาก User-Agent Header
    /// ใช้เพื่อเก็บ Device Info ลงใน ActiveSession
    /// </summary>
    public static class DeviceHelper
    {
        /// <summary>
        /// วิเคราะห์ User-Agent เพื่อแปลงเป็นชื่ออุปกรณ์ (OS + Browser)
        /// </summary>
        public static string ParseDeviceInfo(string userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                return "Unknown Device";

            try
            {
                string os = DetectOS(userAgent);
                string browser = DetectBrowser(userAgent);
                return $"{os} - {browser}";
            }
            catch
            {
                return userAgent.Length > 200 ? userAgent[..200] : userAgent;
            }
        }

        private static string DetectOS(string userAgent)
        {
            if (userAgent.Contains("Windows NT 10")) return "Windows 10";
            if (userAgent.Contains("Windows NT 11")) return "Windows 11";
            if (userAgent.Contains("Windows NT 6.1")) return "Windows 7";
            if (userAgent.Contains("Mac OS X")) return "macOS";
            if (userAgent.Contains("Android")) return "Android";
            if (userAgent.Contains("iPhone")) return "iPhone";
            if (userAgent.Contains("iPad")) return "iPad";
            if (userAgent.Contains("Linux")) return "Linux";
            return "Unknown OS";
        }

        private static string DetectBrowser(string userAgent)
        {
            if (userAgent.Contains("Edg")) return "Microsoft Edge";
            if (userAgent.Contains("Chrome") && !userAgent.Contains("Edg")) return "Chrome";
            if (userAgent.Contains("Firefox")) return "Firefox";
            if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome")) return "Safari";
            if (userAgent.Contains("OPR") || userAgent.Contains("Opera")) return "Opera";
            return "Unknown Browser";
        }

        /// <summary>
        /// (ถ้ามี) แปลงชื่ออุปกรณ์แบบ raw ให้สั้นกระชับ เช่นสำหรับ Mobile
        /// </summary>
        public static string NormalizeDeviceName(string device)
        {
            if (string.IsNullOrWhiteSpace(device))
                return "Unknown";

            return Regex.Replace(device.Trim(), @"\s+", " ");
        }

        public static async Task<string> GetLocationFromIPAsync(string ip)
        {
            try
            {
                using var httpClient = new HttpClient();
                var result = await httpClient.GetFromJsonAsync<dynamic>($"https://ipapi.co/{ip}/json/");
                return $"{result?["city"]}, {result?["country_name"]}";
            }
            catch
            {
                return "Unknown Location";
            }
        }
    }
}
