<template>
  <r-table
    ref="table"
    :controller="controller"
    action=""
    :state-key="stateKey"
    :reload.sync="reloadTable"
    :toolbar="toolbar"
    :paging="paging"
    :headers="headers"
    @rowClicked="onRowClicked"
  >
    <template #examDate="{ item }">
      {{ dateToString(item.examDate, "DD/MM/YYYY") }}
    </template>

    <template #startTime="{ item }">
      {{ dateToString(item.startTime, "hh:mm") }}
    </template>

    <template #endTime="{ item }">
      {{ dateToString(item.endTime, "hh:mm") }}
    </template>

    <template #toolbar-add>
      <v-btn @click="onExamCreate" icon v-show-for.all="[$p.canWriteExam]">
        <v-icon>fa-plus</v-icon>
      </v-btn>
    </template>

    <template #row-actions="{ item }">
      <r-action-buttons
        :actions="tableRowActions"
        :context="item"
        :debounce="300"
        text
      >
        <template #toggle>
          <v-switch
            v-show-for.all="[$p.canWriteExam]"
            :input-value="item.isEnabled"
            hide-details
          />
        </template>
      </r-action-buttons>
    </template>
  </r-table>
</template>

<script lang ="ts">
import { Component, Vue, Watch } from "vue-property-decorator";

import {
  appBarVuexNamespace,
  langVuexNamespace,
  apiService,
  dialogService,
  IAppBarTitleModel,
  IButtonAction,
  ITableFilter,
  ITableHeader,
  ITablePagingOptions,
  ITableToolbarItem,
  Filter,
  FilterOperator,
  RuleOperator,
  RActionButtons,
  RTable,
  DateTimeMixin,
  ITableToolbarFilterBy,
} from "@/container";
import { mixins } from "vue-class-component";
import Create from "./exam-create.vue";
import Update from "./exam-update.vue";
import ExamDetailPanel from "./exam-detail.vue";
import { ExamUpdateModel, IExamDetail } from "./types";
import { namespace } from "vuex-class";

const examVuexNamespace = namespace("exam");

@Component({
  components: {
    RActionButtons,
    RTable,
  },
})
export default class Exam extends mixins(DateTimeMixin) {
  controller = "exams";
  stateKey = "examsTable";
  isPanelOpen: boolean = false;
  selectedExam: IExamDetail | null = null;

  @appBarVuexNamespace.Mutation
  setTitle!: (toolbarTitle: IAppBarTitleModel | null) => void;

  @examVuexNamespace.Mutation
  setCurrentExam!: (exam: IExamDetail | null) => void;

  created() {
    this.setTitle({
      pageContext: this.$t("courseManagement.exams").toString(),
      additionalInfo: this.$t("courseManagement").toString(),
    });
  }

  headers: ITableHeader[] = [
    {
      field: "idExam",
      visible: false,
      visibilityChangeable: false,
    },
    {
      field: "idCourse",
      visible: false,
      visibilityChangeable: false,
    },
    {
      field: "examName",
      localeKey: "courseManagement.exams.examName",
    },
    {
      field: "courseName",
      localeKey: "courseManagement.exams.courseName",
    },
    {
      field: "examDate",
      localeKey: "courseManagement.exams.examDate",
      format: "examDate",
    },
    {
      field: "startTime",
      localeKey: "courseManagement.exams.startTime",
      format: "startTime",
    },
    {
      field: "endTime",
      localeKey: "courseManagement.exams.endTime",
      format: "endTime",
    },
    {
      field: "isEnabled",
      localeKey: "courseManagement.exams.isEnabled",
    },
    {
      format: "row-actions",
      sortable: false,
      visibilityChangeable: false,
      settingsLocaleKey: "app.actions",
      width: "60px",
    },
  ];

  paging: ITablePagingOptions = {
    hasNavigationOutside: true,
    hideNavigationIfOnePageOnly: true,
  };

  textFilterStructure: ITableFilter = {
    filters: [
      Filter.withRule("examName", RuleOperator.IsLike),
      Filter.withRule("courseName", RuleOperator.IsLike),
    ],
    operator: FilterOperator.Or,
  };

  toolbar: ITableToolbarItem[] = [
    {
      format: "search",
      searchProps: {
        filter: this.textFilterStructure,
        minLength: 2,
        textFieldProps: {
          outlined: true,
          dense: true,
        },
      },
    },
    { format: "spacer" },
    { format: "toolbar-add" },
    { format: "export" },
    { format: "settings" },
  ];

  tableRowActions = (context) => {
    if (this.$ps.hasAll([this.$p.canWriteExam])){
      return [
      {
        icon: "fa-edit",
        onAction: this.onExamUpdate,
        disabled: !context.isEnabled,
      },
      {
        icon: "fa-info",
        onAction: this.onExamDetail,
      },
      {
        format: "toggle",
        onAction: this.onExamToggle,
      },
    ] as IButtonAction[];
    }

    return [
      {
        icon: "fa-info",
        onAction: this.onExamDetail,
      },
    ] as IButtonAction[];
  };

  async reloadTable() {
    return;
  }

  async onRowClicked({ item }: { item: IExamDetail }) {
    if (this.selectedExam == null || this.selectedExam.idExam !== item.idExam)
      this.selectedExam = item;

    this.setCurrentExam(this.selectedExam);

    if (this.isPanelOpen) return;

    const panel = dialogService.panel({
      component: ExamDetailPanel,
      containerProps: {
        width: "50%",
        right: true,
        overlay: true,
        entranceDelay: 700,
      },
    });

    this.isPanelOpen = true;

    const result: boolean = await panel.result;

    this.isPanelOpen = false;

    this.setCurrentExam(null);
  }

  async onExamCreate() {
    const saved = await dialogService.popup({
      component: Create,
      containerProps: { width: "50%" },
    }).result;

    if (saved) await this.reloadTable();
  }

  async onExamUpdate(action: IButtonAction, { idExam }): Promise<void> {
    action.loading = true;
    const exam = await apiService.get<ExamUpdateModel>(this.controller, idExam);
    action.loading = false;
    console.log(exam);
    console.log(exam.idExam);

    const saved = await dialogService.popup({
      component: Update,
      componentProps: { exam },
      containerProps: { width: "50%" },
    }).result;

    if (saved) location.reload();
  }

  onExamDetail(action: IButtonAction, { idExam }): void {
    this.$router.push({
      name: "courseManagement.exams.exam",
      params: { id: idExam.toString() },
    });
  }

  async onExamToggle(
    action: IButtonAction,
    { idExam, isEnabled }
  ): Promise<void> {
    console.log(isEnabled);
    await apiService.post(
      this.controller,
      `${idExam}/${isEnabled ? "disable" : "enable"}`
    );

    await this.reloadTable();
  }
}
</script>