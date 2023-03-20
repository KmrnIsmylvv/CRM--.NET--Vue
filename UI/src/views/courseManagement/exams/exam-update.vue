<template>
  <popup-form :actions="actions" :title="$t('courseManagement.examUpdate')">
    <v-container>
      <v-row>
        <v-col cols="12">
          <v-text-field
            v-model="exam.examName"
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
          <r-date-picker
            :label="required($t('courseManagement.exams.examDate'))"
            v-model="exam.examDate"
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
            v-model="exam.startTime"
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
            v-model="exam.endTime"
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
import { Component, Watch, Mixins, Prop } from "vue-property-decorator";
import {
  ValidationMixin,
  IButtonAction,
  apiService,
  DialogMixin,
  RDatePicker,
  RCombobox,
  RTimePicker,
  DateTimeMixin,
} from "@/container";
import { PopupForm } from "@/layouts/default";
import { ExamUpdateModel } from "./types";

@Component({
  components: { PopupForm, RDatePicker, RTimePicker, RCombobox },
})
export default class ExamUpdate extends Mixins(
  DialogMixin,
  ValidationMixin,
  DateTimeMixin
) {
  @Prop({ required: true }) public exam!: ExamUpdateModel;
    

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
    console.log(this.exam);
    console.log(this.exam.idExam);
    this.validateAll();

    if (this.hasErrors) return;

    action.loading = true;

    await this.catchValidationErrorsAsync(() =>
      apiService.post("exams", `${this.exam.idExam}/update`, this.exam)
    );
    if (!this.hasErrors) this.close(true);
  }
  private validateAll() {
    this.validateExamName();
    this.validateStartTime();
    this.validateEndTime();
  }

  private validateExamName() {
    this.removeErrors("examName");
    this.require("examName", this.exam.examName);
  }
  private validateStartTime() {
    this.removeErrors("startTime");
    this.require("startTime", this.exam.startTime);

    if (
      this.exam.startTime !== null &&
      this.exam.endTime !== null &&
      this.exam.startTime > this.exam.endTime
    ) {
      this.addError("startTime", "StartTime must be before than End Date");
    }
  }
  private validateEndTime() {
    this.removeErrors("endTime");
    this.require("endTime", this.exam.endTime);

    if (
      this.exam.endTime !== null &&
      this.exam.startTime !== null &&
      this.exam.endTime < this.exam.startTime
    ) {
      this.addError("endTime", "End Date must be after than Start Date");
    }
  }

  @Watch("model.startTime")
  private onStartTime() {
    this.validateStartTime();
  }
  @Watch("model.endTime")
  private onEndTimeChanged() {
    this.validateEndTime();
  }
}
</script>