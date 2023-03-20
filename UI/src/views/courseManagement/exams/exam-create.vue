

<template>
  <popup-form :actions="actions" :title="$t('courseManagement.examCreate')">
    <v-container>
      <v-row>
        <v-col cols="12">
          <v-text-field
            v-model="model.examName"
            :error="hasError('examName')"
            :error-messages="errorMessages('examName')"
            :hide-details="!hasError('examName')"
            :label="required($t('courseManagement.exams.examName'))"
            placeholder=" "
            dense
            outlined
          >
          </v-text-field>
        </v-col>
        <v-col cols="12">
          <!-- <v-text-field
            v-model="model.idCourse"
            :error="hasError('idCourse')"
            :error-messages="errorMessages('idCourse')"
            :hide-details="!hasError('idCourse')"
            :label="required($t('courseManagement.exams.courseName'))"
            placeholder=" "
            dense
            outlined
          >
          </v-text-field> -->

          <r-combobox
            v-model="model.idCourse"
            controller="courses"
            action="getCoursesNames"
            item-key="idCourse"
            item-value="courseName"
            :error="hasError('idCourse')"
            :error-messages="errorMessages('idCourse')"
            :hide-details="!hasError('idCourse')"
            :label="required($t('courseManagement.exams.courseName'))"
            placeholder=" "
            dense
            outlined
          >
          </r-combobox>
        </v-col>
        <v-col cols="12">
          <r-date-picker
            :label="required($t('courseManagement.exams.examDate'))"
            v-model="model.examDate"
            :error="hasError('examDate')"
            :error-messages="errorMessages('examDate')"
            :hide-details="!hasError('examDate')"
            picker-type="both"
            open-on-icon-click
            placeholder=" "
            dense
            outlined
          >
          </r-date-picker>
        </v-col>
        <v-col cols="6">
          <r-time-picker
            :label="required($t('courseManagement.exams.startTime'))"
            v-model="model.startTime"
            :error="hasError('startTime')"
            :error-messages="errorMessages('startTime')"
            :hide-details="!hasError('startTime')"
            picker-type="both"
            open-on-icon-click
            placeholder=" "
            dense
            outlined
          >
          </r-time-picker>
        </v-col>
        <v-col cols="6">
          <r-time-picker
            :label="required($t('courseManagement.exams.endTime'))"
            v-model="model.endTime"
            :error="hasError('endTime')"
            :error-messages="errorMessages('endTime')"
            :hide-details="!hasError('endTime')"
            picker-type="both"
            open-on-icon-click
            placeholder=" "
            dense
            outlined
          >
          </r-time-picker>
        </v-col>
        <v-col class="py-0"
          ><small>{{ $t("app.mandatoryMarkerExplanation") }}</small></v-col
        >
      </v-row>
    </v-container>
  </popup-form>
</template>


<script lang="ts">
import { Component, Mixins, Watch } from "vue-property-decorator";
import {
  ValidationMixin,
  DialogMixin,
  DateTimeMixin,
  RDatePicker,
  IButtonAction,
  apiService,
  RCombobox,
  RTimePicker,
} from "@/container";
import { PopupForm } from "@/layouts/default";
import { ExamCreateModel } from "./types";
import { watch } from "fs";

@Component({
  components: { PopupForm, RDatePicker, RCombobox, RTimePicker },
})
export default class ExamCreate extends Mixins(
  DialogMixin,
  ValidationMixin,
  DateTimeMixin
) {
  controller = "exams";
  action = "create";
  model = new ExamCreateModel();

  public get actions(): IButtonAction[] {
    return [
      {
        localeKey: "app.save",
        onAction: (action) => this.onSave(action),
        disabled: this.hasErrors,
      },
    ];
  }

  private async onSave(action: IButtonAction): Promise<void> {
    this.validateAll();

    if (this.hasErrors) return;

    action.loading = true;

    await this.catchValidationErrorsAsync(() =>
      apiService.post(this.controller, this.action, this.model)
    );

    if (!this.hasErrors) this.close(true);
  }

  private validateAll() {
    this.validateIdCourse();
    this.validateExamName();
    this.validateExamDate();
    this.validateStartTime();
    this.validateEndTime();
  }
  private validateIdCourse() {
    this.removeErrors("idCourse");
    this.require("idCourse", this.model.idCourse);
  }
  private validateExamName() {
    this.removeErrors("examName");
    this.require("examName", this.model.examName);
  }
  private validateExamDate() {
    this.removeErrors("examDate");
    this.require("examDate", this.model.examDate);
  }

  private validateStartTime() {
    this.removeErrors("startTime");
    this.require("startTime", this.model.startTime);

    if (
      this.model.startTime !== null &&
      this.model.endTime !== null &&
      this.model.startTime > this.model.endTime
    )
      this.addError("startTime", "Start Time must be less than End Time");
  }

  private validateEndTime() {
    this.removeErrors("endTime");
    this.require("endTime", this.model.endTime);

    if (
      this.model.endTime !== null &&
      this.model.startTime !== null &&
      this.model.endTime < this.model.startTime
    )
      this.addError("endDate", "End Date must be greater than Start Date");
  }

  @Watch("model.idCourse")
  private onIdCourseChanged() {
    this.validateIdCourse();
  }
  @Watch("model.examName")
  private onExamNameChanged() {
    this.validateExamName();
  }
  @Watch("model.startTime")
  private onCourseStartTimeChanged() {
    this.validateStartTime();
  }
  @Watch("model.endTime")
  private onCourseEndTimeChanged() {
    this.validateEndTime();
  }
}
</script>
