
let preset = [];


// Preset for Button ///

const buttonPreset = [
  {
    key : '--p-button-primary-color',
    value : '#fff'
  },
  {
    key : '--p-button-primary-background',
    value : '#0d6efd'
  },
  {
    key : '--p-button-primary-border-color',
    value : '#0d6efd'
  },
  {
    key : '--p-button-primary-hover-color',
    value : '#fff'
  },
  {
    key : '--p-button-primary-hover-background',
    value : '#0b5ed7'
  },
  {
    key : '--p-button-primary-hover-border-color',
    value : '#0a58ca'
  },
  {
    key : '--p-button-primary-active-color',
    value : '#fff'
  },
  {
    key : '--p-button-primary-active-background',
    value : '#0a58ca'
  },
  {
    key : '--p-button-primary-active-border-color',
    value : '#0a53be'
  },

  {
    key : '--p-button-secondary-color',
    value : '#fff'
  },
  {
    key : '--p-button-secondary-background',
    value : '#6c757d'
  },
  {
    key : '--p-button-secondary-border-color',
    value : '#6c757d'
  },
  {
    key : '--p-button-secondary-hover-color',
    value : '#fff'
  },
  {
    key : '--p-button-secondary-hover-background',
    value : '#5c636a'
  },
  {
    key : '--p-button-secondary-hover-border-color',
    value : '#565e64'
  },
  {
    key : '--p-button-secondary-active-color',
    value : '#fff'
  },
  {
    key : '--p-button-secondary-active-background',
    value : '#565e64'
  },
  {
    key : '--p-button-secondary-active-border-color',
    value : '#51585e'
  },


  {
    key : '--p-button-success-color',
    value : '#fff'
  },
  {
    key : '--p-button-success-background',
    value : '#198754'
  },
  {
    key : '--p-button-success-border-color',
    value : '#198754'
  },
  {
    key : '--p-button-success-hover-color',
    value : '#fff'
  },
  {
    key : '--p-button-success-hover-background',
    value : '#157347'
  },
  {
    key : '--p-button-success-hover-border-color',
    value : '#146c43'
  },
  {
    key : '--p-button-success-active-color',
    value : '#fff'
  },
  {
    key : '--p-button-success-active-background',
    value : '#146c43'
  },
  {
    key : '--p-button-success-active-border-color',
    value : '#13653f'
  },


  {
    key : '--p-button-danger-color',
    value : '#fff'
  },
  {
    key : '--p-button-danger-background',
    value : '#dc3545'
  },
  {
    key : '--p-button-danger-border-color',
    value : '#dc3545'
  },
  {
    key : '--p-button-danger-hover-color',
    value : '#fff'
  },
  {
    key : '--p-button-danger-hover-background',
    value : '#bb2d3b'
  },
  {
    key : '--p-button-danger-hover-border-color',
    value : '#b02a37'
  },
  {
    key : '--p-button-danger-active-color',
    value : '#fff'
  },
  {
    key : '--p-button-danger-active-background',
    value : '#b02a37'
  },
  {
    key : '--p-button-danger-active-border-color',
    value : '#a52834'
  },

  {
    key : '--p-button-info-color',
    value : '#333'
  },
  {
    key : '--p-button-info-background',
    value : '#0dcaf0'
  },
  {
    key : '--p-button-info-border-color',
    value : '#0dcaf0'
  },
  {
    key : '--p-button-info-hover-color',
    value : '#333'
  },
  {
    key : '--p-button-info-hover-background',
    value : '#31d2f2'
  },
  {
    key : '--p-button-info-hover-border-color',
    value : '#25cff2'
  },
  {
    key : '--p-button-info-active-color',
    value : '#333'
  },
  {
    key : '--p-button-info-active-background',
    value : '#3dd5f3'
  },
  {
    key : '--p-button-info-active-border-color',
    value : '#25cff2'
  },


  {
    key : '--p-button-warn-color',
    value : '#333'
  },
  {
    key : '--p-button-warn-background',
    value : '#ffc107'
  },
  {
    key : '--p-button-warn-border-color',
    value : '#ffc107'
  },
  {
    key : '--p-button-warn-hover-color',
    value : '#333'
  },
  {
    key : '--p-button-warn-hover-background',
    value : '#ffca2c'
  },
  {
    key : '--p-button-warn-hover-border-color',
    value : '#ffc720'
  },
  {
    key : '--p-button-warn-active-color',
    value : '#333'
  },
  {
    key : '--p-button-warn-active-background',
    value : '#ffcd39'
  },
  {
    key : '--p-button-warn-active-border-color',
    value : '#ffc720'
  },



  // {
  //   key : '--p-button-success-background',
  //   value : '#198754'
  // },
  // {
  //   key : '--p-button-success-border-color',
  //   value : '#198754'
  // },
  //  {
  //   key : '--p-button-success-active-background',
  //   value : '#198754'
  // },
  //  {
  //   key : '--p-button-success-active-border-color',
  //   value : '#198754'
  // },
  // {
  //   key : '--p-button-secondary-color',
  //   value : '#fff'
  // },
  // {
  //   key : '--p-button-secondary-background',
  //   value : '#6c757d'
  // },
  // {
  //   key : '--p-button-secondary-border-color',
  //   value : '#6c757d'
  // },
  // {
  //   key : '--p-button-danger-color',
  //   value : '#fff'
  // },
  // {
  //   key : '--p-button-danger-background',
  //   value : '#dc3545'
  // },
  // {
  //   key : '--p-button-danger-border-color',
  //   value : '#dc3545'
  // },
  // {
  //   key : '--p-button-warn-color',
  //   value : '#333'
  // },
  // {
  //   key : '--p-button-warn-background',
  //   value : '#ffca2c'
  // },
  // {
  //   key : '--p-button-warn-border-color',
  //   value : '#ffca2c'
  // }
]


preset = [...preset, ...buttonPreset]




export const colorPreset = preset;
