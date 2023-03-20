<template>
  <v-container fluid>
    <v-row>
      <v-col cols="12">
        <v-text-field
          v-model="exam.examName"
          :label="$t('courseManagement.exams.examName')"
          hide-details="true"
          dense
          outlined
          readonly
        >
        </v-text-field>
      </v-col>

      <v-col>
        <v-text-field
          v-model="exam.courseName"
          :label="$t('courseManagement.exams.courseName')"
          hide-details="true"
          dense
          outlined
          readonly
        >
        </v-text-field>
      </v-col>

      <v-col cols="6">
        <r-date-picker
          :label="$t('courseManagement.exams.examDate')"
          v-model="exam.examDate"
          picker-type="both"
          :hide-details="true"
          dense
          outlined
          readonly
        >
        </r-date-picker>
      </v-col>

      <v-col cols="6">
        <r-time-picker
          :label="$t('courseManagement.exams.startTime')"
          v-model="exam.startTime"
          picker-type="both"
          :hide-details="true"
          dense
          outlined
          readonly
        >
        </r-time-picker>
      </v-col>

      <v-col cols="6">
        <r-time-picker
          :label="$t('courseManagement.exams.endTime')"
          v-model="exam.endTime"
          picker-type="both"
          :hide-details="true"
          dense
          outlined
          readonly
        >
        </r-time-picker>
      </v-col>

      <v-col cols="12">
        <v-checkbox
          v-model="exam.isEnabled"
          :label="$t('courseManagement.exams.isEnabled')"
          hide-details="true"
          dense
          outlined
          readonly
        >
        </v-checkbox>
      </v-col>
    </v-row>
  </v-container>
</template>


<script lang="ts">
import { Component, Mixins, Vue, Watch } from "vue-property-decorator";

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
  RDatePicker,
  ITableToolbarFilterBy,
  INavigationButtonModel,
  RTimePicker,
} from "@/container";
import { ExamDetail } from "../types";

@Component({
  components: {
    RDatePicker,
    RTimePicker,
  },
})
export default class Exam extends Mixins(DateTimeMixin) {
  idExam = this.$route.params.id;
  exam: ExamDetail = {
    courseName: null,
    examName: null,
    examDate: null,
    startTime: null,
    endTime: null,
    isEnabled: null,
  };

  @appBarVuexNamespace.Mutation
  setTitle!: (toolbarTitle: IAppBarTitleModel | null) => void;

  @appBarVuexNamespace.Mutation
  setNavigationButton!: (navigationButton: INavigationButtonModel) => void;

  @appBarVuexNamespace.Mutation
  clearNavigationButton!: () => void;

  async created() {
    this.exam = await apiService.get<ExamDetail>("exams", this.idExam);

    this.setTitle({
      pageContext: `${this.$t("courseManagement.exam").toString()} ${
        this.idExam
      }`,
      additionalInfo: this.$t("courseManagement").toString(),
    });

    this.setNavigationButton({
      routeName: "courseManagement.exams",
      icon: "fa-arrow-left",
      iconProps: { color: "white" },
    });
  }

  destroyed() {
    this.clearNavigationButton();
  }
}
</script>
<route>
{
    "includeInDrawer": false
}
</route>

