import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, PLATFORM_ID, ViewChild } from '@angular/core';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { Subscription, lastValueFrom } from 'rxjs';
import { Customer, CustomerService, Representative } from 'src/app/Shared/Services/customerservice';
import { FormsModule } from '@angular/forms';
import { CommonModule, CurrencyPipe, DatePipe, isPlatformBrowser } from '@angular/common';

import { COLOR_BAR, COLOR_BAR_HOVER, COLOR_PIE, COLOR_PIE_HOVER, ContractList, DashboardService, WorkList } from './dashboard-service';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ChartOptions } from 'chart.js';

import ChartDataLabels from 'chartjs-plugin-datalabels';
import { Chart } from 'chart.js';
import { SecurityService } from '../securities/security.service';
import { HttpClient } from '@angular/common/http';
import { FilterService, GridModule, PageService, SortService } from '@syncfusion/ej2-angular-grids';
import { ApiService } from 'src/app/Shared/Services/api.service';
import { SelectComponent, SelectOption } from 'src/app/Shared/Components/select/select.component';
import { TooltipDirective } from "src/app/Shared/Directives/tooltip.directive";
import { ShareDirectiveModule } from 'src/app/Shared/Directives/share-directive.module';
Chart.register(ChartDataLabels);

@Component({
    selector: 'app-dashboard',
    imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    GridModule,
    ShareDirectiveModule
],
    providers: [CustomerService, DashboardService,PageService,
                SortService,
                FilterService,],
    templateUrl: './dashboard.component.html',
    styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy {
  data : any = null;


  @ViewChild('pieChart') pieChartRef!: any;
  @ViewChild('barChart') barChartRef!: any;

  customers!: Customer[];

  representatives!: Representative[];

  statuses!: any[];

  loading: boolean = true;

  activityValues: number[] = [0, 100];

  searchValue: string | undefined;
  value = '';

  chartData_bar: any;
  chartData_pie: any;

  options_bar: any;
  options_pie: ChartOptions;

  platformId = inject(PLATFORM_ID);

  $contractList : Subscription = null;
  $worklist : Subscription = null;

  contractLists : ContractList[] = [];
  workLists : WorkList[] = [];

  formatContracts: any[] = [];
  formatWorkLists: any[] = [];

  totalAllWorklist: number = null;
  totalNew: number = null;
  totalInProgress: number = null;
  totalSuccess: number = null;

  status_allWorklists = [1,4];
  status_new = [6];
  status_inprogress = [7,8,15,16,17];
  status_success = [9,10];

  contractStatusList = [];
  followupStatusList = [];


  piePlugins = [ChartDataLabels];

  constructor(
    private http: HttpClient,
    private loaderService : AppLoaderService,
    private dashboardService: DashboardService,
    private translate: TranslateService,
    private cd: ChangeDetectorRef,
    private securityService : SecurityService,
    private apiService : ApiService

  ){

    this.translate.onLangChange.subscribe(item => {
        if(this.formatWorkLists.length > 0){
          this.pieChartRef.chart.data.labels = this.formatContracts.map( item => item.statusText[this.translate.currentLang]);
          this.barChartRef.chart.data.datasets[0].label = this.translate.instant('Num of Contract'),
          this.pieChartRef.chart.update();
        }

        if(this.formatContracts.length > 0){
          this.barChartRef.chart.data.labels = this.formatWorkLists.map( item => item.statusText[this.translate.currentLang]);
          this.barChartRef.chart.data.datasets[0].label = this.translate.instant('Num of Work');
          this.barChartRef.chart.update();
        }
    })
    // this.$contractList = this.dashboardService.getContractList().subscribe((data) => {
    //     this.contractLists = data;
    // });

    // this.$worklist = this.dashboardService.getWorkList().subscribe((data) => {
    //     this.workLists = data;
    // });

  }

  async ngOnInit() {
    const loaderID = this.loaderService.show();
    try {
        // await this.dashboardService.getContractListData();
        // await this.dashboardService.getWorkListData();
        // this.contractStatusList = await this.dashboardService.getDashboardContractStatus();
        // this.followupStatusList = await this.dashboardService.getDashboardFollowupStatus();
        // this.getData();
        // this.initChart();

    } catch(e){}
    finally {
      this.loaderService.hide(loaderID);
    }


  }


    onCityChange(value: any) {
    console.log('Selected City:', value);
  }

  getCountData(rowData: any, filteredField: string, mappingField : any , mapping: any) {
    const summary: { [key: string]: number } = {};
    rowData.forEach(item => {
        const status = item[filteredField];
        if (status) {
            summary[status] = (summary[status] || 0) + 1;
        }
    });


    const result = mapping.map(item => {
        return {
                  statusCode: Number(item[mappingField.id]),
                  status : item[mappingField.label],
                  statusText : { th : item[mappingField.label] , en : item[mappingField.key] },
                  count: summary[item[mappingField.key]] || 0
                }
    });

    return result;
  }


  getData(){
    this.formatContracts = this.getCountData(this.contractLists, 'contractStatusDesc', { id : 'contractStatusID' , key : 'contractStatusCode' , label : 'contractStatusDesc' }, this.contractStatusList);
    this.formatWorkLists = this.getCountData( this.workLists, 'followupStatusDesc',  { id : 'followupStatusID' , key : 'followupStatusCode' , label : 'followupStatusDesc' }, this.followupStatusList)
    this.totalAllWorklist = this.formatWorkLists.reduce((sum, x) =>  sum + x.count,0);
    this.totalNew = this.formatWorkLists.filter(item => this.status_new.includes(item.statusCode)).reduce((sum, x) =>  sum + x.count,0)
    this.totalInProgress = this.formatWorkLists.filter(item => this.status_inprogress.includes(item.statusCode)).reduce((sum, x) =>  sum + x.count,0);
    this.totalSuccess = this.formatWorkLists.filter(item => this.status_success.includes(item.statusCode)).reduce((sum, x) =>  sum + x.count,0);
  }

  initChart() {
      if (isPlatformBrowser(this.platformId)) {
          const documentStyle = getComputedStyle(document.documentElement);
          const textColor = documentStyle.getPropertyValue('--p-text-color');
          const textColorSecondary = documentStyle.getPropertyValue('--p-text-muted-color');
          const surfaceBorder = documentStyle.getPropertyValue('--p-content-border-color');

          this.chartData_bar = {
              labels: this.formatWorkLists.map( item => item.statusText[this.translate.currentLang]),
              datasets: [
                  {
                      label: this.translate.instant('Num of Work'),
                      backgroundColor: COLOR_BAR,
                      borderColor: COLOR_BAR_HOVER,
                      data: this.formatWorkLists.map( item => item.count)
                  }
              ]
          };

          this.chartData_pie = {
            labels: this.formatContracts.map( item => item.statusText[this.translate.currentLang]),
            datasets: [
                  {
                      label: this.translate.instant('Num of Contract'),
                      backgroundColor: COLOR_PIE,
                      hoverBackgroundColor: COLOR_PIE_HOVER,
                      data: this.formatContracts.map( item => item.count)
                  }
              ]
          };

          this.options_pie = {
                maintainAspectRatio : false,
                responsive : true,
                aspectRatio : 0.5,
                plugins: {
                    legend: {
                        labels: {
                            usePointStyle: true,
                            color: textColor,

                        },
                        position : 'bottom',
                    },
                    datalabels: {
                      color: '#fff',
                      anchor: 'center',
                      align: 'center',
                      font: {
                        size: 14,
                        weight: 'bold'
                      },
                      formatter: (value: number , context) =>  { return `${value}`; }
                    },



                }
            };
          this.options_bar = {
              indexAxis: 'y',
              responsive : true,
              maintainAspectRatio: false,
              aspectRatio: 0.5,
              plugins: {
                  legend: {
                      labels: {
                          color: textColor
                      },
                      display: false
                  },
                  datalabels: {
                      color: '#333',
                      anchor: 'end',
                      align: 'end',
                      font: {
                        size: 14,
                        weight: 'bold'
                      },
                      formatter: (value: number , context) => { return `${value}`; }
                    },
              },
              scales: {
                  x: {
                      ticks: {
                          color: textColorSecondary,
                          font: {
                              weight: 500
                          }
                      },
                      grid: {
                          color: surfaceBorder,
                          drawBorder: false
                      }
                  },
                  y: {
                      ticks: {
                          color: textColorSecondary
                      },
                      grid: {
                          color: surfaceBorder,
                          drawBorder: false
                      }
                  }
              }
          };

          this.cd.markForCheck()
      }
  }

   ngOnDestroy(): void {
      this.$contractList?.unsubscribe();
      this.$worklist?.unsubscribe();
  }
}
