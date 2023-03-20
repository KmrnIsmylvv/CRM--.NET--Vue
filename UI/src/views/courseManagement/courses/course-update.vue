<template>
  <popup-form :title="$t('courseManagement.courseUpdate')" :actions="actions">
    <v-container>
      <v-row>
        <v-col cols="12">
          <v-text-field
            :label="required($t('courseManagement.courses.name'))"
            v-model="course.name"
            :error="hasError('name')"
            :error-messages="errorMessages('name')"
            :hide-details="!hasError('name')"
            outlined
            dense
            placeholder=" "
          >
          </v-text-field>
        </v-col>
        <!-- <v-col cols=12>
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
        </v-col> -->
        <v-col cols="12">
          <v-text-field
            :label="$t('courseManagement.courses.description')"
            v-model="course.description"
            outlined
            dense
            :hide-details="!hasError('description')"
            placeholder=" "
          >
          </v-text-field>
        </v-col>
        <v-col cols="12">
          <r-date-picker
            :label="required($t('courseManagement.courses.startDate'))"
            open-on-icon-click
            v-model="course.startDate"
            :error="hasError('startDate')"
            :error-messages="errorMessages('startDate')"
            :hide-details="!hasError('startDate')"
            outlined
            dense
            placeholder=" "
          >
          </r-date-picker>
        </v-col>
        <v-col cols="12">
          <r-date-picker
            :label="required($t('courseManagement.courses.endDate'))"
            open-on-icon-click
            v-model="course.endDate"
            :error="hasError('endDate')"
            :error-messages="errorMessages('endDate')"
            :hide-details="!hasError('endDate')"
            outlined
            dense
            placeholder=" "
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
import { Component, Watch, Mixins, Prop } from "vue-property-decorator";
import {
  ValidationMixin,
  IButtonAction,
  apiService,
  DialogMixin,
  RDatePicker,
  RCombobox,
} from "@/container";
import { PopupForm } from "@/layouts/default";
import { CourseUpdateModel } from "./types";

@Component({
  components: { PopupForm, RDatePicker, RCombobox },
})
export default class CourseUpdate extends Mixins(ValidationMixin, DialogMixin) {
  @Prop({ required: true }) public course!: CourseUpdateModel;

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
      apiService.post("courses", `${this.course.idCourse}/update`, this.course)
    );

    if (!this.hasErrors) this.close(true);
  }

  private validateAll() {
    this.validateName();
    this.validateStartDate();
    this.validateEndDate();
  }

  private validateName() {
    this.removeErrors("name");
    this.require("name", this.course.name);
  }

  private validateStartDate() {
    this.removeErrors("startDate");
    this.require("startDate", this.course.startDate);
    if (
      this.course.startDate !== null &&
      this.course.endDate !== null &&
      this.course.startDate > this.course.endDate
    )
      this.addError("startDate", "Start Date must be less than End Date");
  }

  private validateEndDate() {
    this.removeErrors("endDate");
    this.require("endDate", this.course.endDate);
    if (
      this.course.endDate !== null &&
      this.course.startDate !== null &&
      this.course.endDate < this.course.startDate
    )
      this.addError("endDate", "End Date must be greater than Start Date");
  }

  @Watch("course.name")
  private onNameChanged() {
    this.validateName();
  }

  @Watch("course.startDate")
  private onStartDateChanged() {
    this.validateStartDate();
  }

  @Watch("course.endDate")
  private onEndDateChanged() {
    this.validateEndDate();
  }
}
</script>