export interface Task {
  worklistId?: string
  contractNo?: string
  customerName?: string
  productType?: string
  contractStatus?: string
  bucket?: number
  paymentDueDate?: string
  dayPastDue?: number
  overdueAmount?: number
  outstandingAmount?: number
  areaCode?: string
  collectorName?: string
  reassignBy?: string
  reassignDate?: Date
  dueActionDate?: Date
  followupStatus?: string
  lastFollowupDate?: string
  remark?: string;
  nationalID?: string;
  primaryPhoneNo?: string;
  secondaryPhoneNo?: string;
  email?: string;
  area?: string;
  dueDate?: Date
  assignDate?: Date
  fullName?: string
  followupAction?: string,
  lastPaymentDate?: Date,
  outstandingBalance?: string;
}



export interface ContractDescription{
  contractStartDate?: Date
  contractEndDate?: Date
  loanAmount?: number
  installAmount?: number
  contractTerm?: number
  interestRate?: number
  interestRateType?: string
  contractDueDay?: number
  assetGroup?: string
  assetType?: Date
  assetDescription?: string
  assetPrice?: number
  brand?: string
  model?: string
  series?: string
  year?: string
  engineNo?: string
  chassisNo?: string
  plateNo?: string
}

export interface ContractDetail{
  contractNo?: string
  contractStartDate?: Date
  contractEndDate?: Date
  loanAmount?: number
  installAmount?: number
  term?: number
  interestRate?: number
  interestRateType?: string
  contractDueDay?: number
  assetGroup?: string
  assetType?: Date
  description?: string
  assetPrice?: number
  brand?: string
  model?: string
  series?: string
  year?: string
  engineNo?: string
  chassisNo?: string
  plateNo?: string,
  dueDate?: Date
  registerProvince?: string
}


export interface OutstandingBalance{
  loanAmount?: number
  outstandingAmount?: number
  totalOverdues?: number
}

export interface OutstandingBalanceList{
  rowIndex?: number
  overdueType?: string
  overdueTerm?: string
  overdueAmount?: number
}



export interface CollectionNote{
  followupDate?: Date
  followupAction?: string
  followupResult?: string
  contactPerson?: string
  contactPhone?: string
  nextFollowupDate?: Date
  promiseToPayDate?: Date
  promiseToPayAmount?: number
  collectionRemark?: string
  updatedBy?: string;
}

export interface ApplicantandGuarantor{
  firstName?: string;
  idCard?: string;
  dateOfBirth?: Date;
  age?: string;
  gender?: string;
  maritalStatus?: string;
  occupation?: string;
  income?: number;
  workPlace?: string;
  email?: string;
  relationship?: string;
}

export interface Address{
  personType?: string;
  addressOwner?: string;
  addressType?: string;
  address?: string;
  isVerifiedAddress?: any;
  verifiedDate?: Date;
  addressRemark?: string;
}

export interface ContractPhone{
  personType?: string;
  phoneOwner?: string;
  phoneType?: string;
  phoneNo?: string;
  isVerifiedPhone?: any;
  verifiedDate?: Date;
  phoneRemark?: string;
}

export interface ContractPayment {
  ContractTerm?: number;
  PaidTerm?: number;
  PaidAmount?: number;
  PaymentDate?: Date;
  PaymentAmount?: number;
  PaymentChannel?: string;
  ReferenceNo?: string;
  InstallAmount?: string;
  PenaltyAmount?: number;
  OtherFeesAmount?: string;
}












