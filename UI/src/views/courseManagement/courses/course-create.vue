

<template>
  <popup-form :actions="actions" :title="$t('courseManagement.courseCreate')">
    <v-container>
      <v-row>
        <v-col cols="12">
          <v-text-field
            v-model="model.name"
            :error="hasError('name')"
            :error-messages="errorMessages('name')"
            :hide-details="!hasError('name')"
            :label="required($t('courseManagement.courses.name'))"
            placeholder=" "
            dense
            outlined
          >
          </v-text-field>
        </v-col>
        <v-col cols="12">
          <v-text-field
            v-model="model.description"
            :label="$t('courseManagement.courses.description')"
            :hide-details="!hasError('description')"
            placeholder=" "
            dense
            outlined
          >
          </v-text-field>
        </v-col>
        <v-col cols="12">
          <!-- <v-text-field
            v-model="model.idTeacher"
            :error="hasError('idTeacher')"
            :error-messages="errorMessages('idTeacher')"
            :hide-details="!hasError('idTeacher')"
            :label="required($t('courseManagement.courses.teacher'))"
            placeholder=" "
            dense
            outlined
          >
          </v-text-field> -->

          <r-combobox
            v-model="model.idTeacher"
            controller="system"
            action="getTeachers"
            item-key="idTeacher"
            item-value="teacherName"
            :error="hasError('idTeacher')"
            :error-messages="errorMessages('idTeacher')"
            :hide-details="!hasError('idTeacher')"
            :label="required($t('courseManagement.courses.teacher'))"
            placeholder=" "
            dense
            outlined
          >
          </r-combobox>
        </v-col>
        <v-col cols="6">
          <r-date-picker
            :label="required($t('courseManagement.courses.startDate'))"
            v-model="model.startDate"
            :error="hasError('startDate')"
            :error-messages="errorMessages('startDate')"
            :hide-details="!hasError('startDate')"
            picker-type="both"
            open-on-icon-click
            placeholder=" "
            dense
            outlined
          >
          </r-date-picker>
        </v-col>
        <v-col cols="6">
          <r-date-picker
            :label="required($t('courseManagement.courses.endDate'))"
            v-model="model.endDate"
            :error="hasError('endDate')"
            :error-messages="errorMessages('endDate')"
            :hide-details="!hasError('endDate')"
            picker-type="both"
            open-on-icon-click
            placeholder=" "
            dense
            outlined
          >
          </r-date-picker>
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
} from "@/container";
import { PopupForm } from "@/layouts/default";
import { CourseCreateModel } from "./types";
import { watch } from "fs";

@Component({
  components: { PopupForm, RDatePicker, RCombobox },
})
export default class CourseCreate extends Mixins(
  DialogMixin,
  ValidationMixin,
  DateTimeMixin
) {
  controller = "courses";
  action = "create";
  model = new CourseCreateModel();

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
    this.validateIdTeacher();
    this.validateCourseName();
    this.validateStartDate();
    this.validateEndDate();
  }
  private validateIdTeacher() {
    this.removeErrors("idTeacher");
    this.require("idTeacher", this.model.idTeacher);
  }
  private validateCourseName() {
    this.removeErrors("name");
    this.require("name", this.model.name);
  }
  private validateStartDate() {
    this.removeErrors("startDate");
    this.require("startDate", this.model.startDate);

    if (
      this.model.startDate !== null &&
      this.model.endDate !== null &&
      this.model.startDate > this.model.endDate
    )
      this.addError("startDate", "Start Date must be less than End Date");
  }

  private validateEndDate() {
    this.removeErrors("endDate");
    this.require("endDate", this.model.endDate);

    if (
      this.model.endDate !== null &&
      this.model.startDate !== null &&
      this.model.endDate < this.model.startDate
    )
      this.addError("endDate", "End Date must be greater than Start Date");
  }

  @Watch("model.idTeacher")
  private onIdTeacherChanged() {
    this.validateIdTeacher();
  }
  @Watch("model.name")
  private onCourseNameChanged() {
    this.validateCourseName();
  }
  @Watch("model.startDate")
  private onCourseStartDateChanged() {
    this.validateStartDate();
  }
  @Watch("model.endDate")
  private onCourseEndDateChanged() {
    this.validateEndDate();
  }
}
</script>
