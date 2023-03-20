<template>
  <panel-form :title="$t('courseManagement.examDetail')">
    <v-container>
      <v-row>
        <v-col cols="12">
          <v-text-field
            :label="$t('courseManagement.exams.examName')"
            v-model="exam.examName"
            :hide-details="true"
            outlined
            dense
            readonly
          >
          </v-text-field>
        </v-col>
        <v-col cols="12">
          <r-date-picker
            :label="$t('courseManagement.exams.examDate')"
            open-on-icon-click
            v-model="exam.examDate"
            :hide-details="true"
            outlined
            dense
            readonly
          >
          </r-date-picker>
        </v-col>
        <v-col cols="12">
          <r-time-picker
            :label="$t('courseManagement.exams.startTime')"
            open-on-icon-click
            v-model="exam.startTime"
            :hide-details="true"
            outlined
            dense
            readonly
          >
          </r-time-picker>
        </v-col>
        <v-col cols="12">
          <r-time-picker
            :label="$t('courseManagement.exams.endTime')"
            open-on-icon-click
            v-model="exam.endTime"
            :hide-details="true"
            outlined
            dense
            readonly
          >
          </r-time-picker>
        </v-col>
      </v-row>
    </v-container>
  </panel-form>
</template>


<script lang="ts">
import { Component, Watch, Mixins, Prop } from "vue-property-decorator";
import {
  IButtonAction,
  apiService,
  DialogMixin,
  RDatePicker,
  RTimePicker,
} from "@/container";
import { PanelForm } from "@/layouts/default/containers";
import { namespace } from "vuex-class";
import { IExamDetail } from "./types";

const examVuexNamespace = namespace("exam");

@Component({
  components: { PanelForm, RDatePicker, RTimePicker },
})
export default class ExamDetail extends Mixins(DialogMixin) {
  @examVuexNamespace.State exam!: IExamDetail;
}
</script>



