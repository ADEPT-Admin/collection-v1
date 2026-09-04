export enum Action {
  New = 'new',
  Set = 'set',
  Delete = 'del',
  GetList = 'getlist',
  Get = 'get',
  GetX = 'getx',
  DET = 'del',
  Paste = 'paste',
  Reset = 'reset',
  Regress = 'regress',
  Run = 'run',
  Search = 'search',
  Delchildren ='delchildren',
  GetAll = 'getall',
  Logoff = 'logoff',
  Print = 'print'
}

export enum Type {
  ValueList = 'ValueList',
  CategoriesList = 'CategoriesList',
  DataLayout = 'DataLayout',
  Project = 'Project',
  DataDef = 'DataDef',
  ProjectFamily = 'ProjectFamily',
  User = 'User',
  ResourceIndex = 'ResourceIndex',
  ReasonList = 'ReasonList',
  SegTree = 'SegTree',
  TreatmentList = 'TreatmentList',
  Scorecard = 'Scorecard',
  Function = 'Function',
  TestCase = 'TestCase',
  TestScript = 'TestScript',
  RuleSet = 'RuleSet',
  Rule = 'Rule',
  Process = 'Process',
  List = 'List',
  UserGroup = 'UserGroup',
  Permission = 'Permission',
  ParameterizedFunction = 'ParameterizedFunction',
  IndexedGroupField = 'IndexedGroupField',
  DecTree = 'DecTree',
}

export enum RoutingUrl {
  AdverseActionCodes = 'adverseactioncodes',
  DataDictionary = 'datadictionary',
  Treatments = 'treatments',
  ReportLayout = 'reportlayout',
  ResponseLayout = 'responselayout',
  RequestLayout = 'requestlayout',
  RuleSets = 'rulesets',
  ScorecardsExclusions = 'scorecardsexclusions'

}

export enum Separator {
  Comma = 'comma'
}

export enum DialogType{
  Error = 'error',
  Warning = 'warning',
  Notification = 'notification',
  Confirm = 'confirm',
  ConfirmRestore = 'confirmRestore',
  New = 'new',
  Close = 'close',
  Success = 'success',
  Info = 'info',
   Question = 'question'
}

export enum Icon {
  Confirmed =`<i class="fa fa-check"></i>`,
  Cancel = `<i class="fa fa-times"></i>`,
}

export enum Color {
  Blue = '#0378d5',
}


export const reserveWord = [
  'Input',
  'Response',
  'Portfolio Assignment',
  'Experimental Group Assignment',
  'Scorecard Assignment',
  'ScoringExclusion',
];


export const mainRouting  = {
  '0100' : 'projectcontrol',
  '0200' : 'decisioncontrol',
  '0400' : 'portfoliocontrol',
  '0500' : 'scorecardcontrol',
  '0600' : 'testcontrol',
  '0700' : 'datacontrol',
  '0800' : 'componentcontrol',
  '0900' : 'systemcontrol',
  '1000' : 'admincontrol',
}


export const blackListUrl = [
  '/selectproject'
]

export enum StageDescription{
  CreatedBy = "CreatedBy",
  LockedBy = "Locked for Testing by",
  PromotedTo = 'Promoted to',
  ArchivedBy = 'Archived by',
  By = 'by'
}
