

<template>
  <popup-form :actions="actions" :title="$t('courseManagement.lessonCreate')">
    <v-container>
      <v-row>
        <v-col cols="12">
          <v-text-field
            v-model="model.name"
            :hide-details="true"
            :label="required($t('courseManagement.lessons.name'))"
            placeholder=" "
            dense
            outlined
          >
          </v-text-field>
        </v-col>
        <v-col cols="12">
          <v-text-field
            v-model="model.description"
            :label="$t('courseManagement.lessons.description')"
            :hide-details="true"
            placeholder=" "
            dense
            outlined
          >
          </v-text-field>
        </v-col>
        <v-col cols="12">
          <v-text-field
            v-model="model.duration"
            :label="$t('courseManagement.lessons.duration')"
            :hide-details="true"
            placeholder=" "
            dense
            outlined
          >
          </v-text-field>
        </v-col>
        <v-col class="py-0"
          ><small>{{ $t("app.mandatoryMarkerExplanation") }}</small></v-col
        >
      </v-row>
    </v-container>
  </popup-form>
</template>


<script lang="ts">
import { Component, Mixins, Prop, Watch } from "vue-property-decorator";
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
import { watch } from "fs";
import { LessonCreateModel } from "../types";

@Component({
  components: { PopupForm, RDatePicker, RCombobox },
})
export default class LessonCreate extends Mixins(DialogMixin, ValidationMixin) {
  @Prop({ required: true }) public idCourse!: number;

  controller = "courses";
  action = `${this.idCourse}/lessons/add`;
  model = new LessonCreateModel();

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
    if (this.hasErrors) return;

    action.loading = true;

    await this.catchValidationErrorsAsync(() =>
      apiService.post(this.controller, this.action, this.model)
    );

    if (!this.hasErrors)
     this.close(true);

  }
}
</script>
