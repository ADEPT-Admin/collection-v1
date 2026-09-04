export enum ReferenceType {
  SegTree = 'SegTree',
  Function = 'Function',
  Scorecard = 'Scorecard',
  Treatment = 'Treatment',
  Value = 'Value',
  DecTree = 'decTree'
}

export enum ReferenceTypeNotFound {
  S = '*** Scorecard LABEL NOT FOUND ****',
  B = '*** Function LABEL NOT FOUND ****',
  D = '*** SegTree LABEL NOT FOUND ****',
  T = '*** Treatment=None not found***',
  DD = '*** Decision Tree LABEL NOT FOUND ****',
  Value = '*** Value=None not found***'
}

export enum ActionMoveMode {
  GoPrev = 'GO_PREV',
  GoNext = 'GO_NEXT',
}

export enum DecKeyTypes {
  Discrete = 'discrete',
  UnDiscrete = 'continuous',
}
