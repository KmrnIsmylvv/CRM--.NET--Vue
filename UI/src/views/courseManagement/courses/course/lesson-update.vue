<template>
  <popup-form :title="$t('courseManagement.lessonUpdate')" :actions="actions">
    <v-container>
      <v-row>
        <v-col cols="12">
          <v-text-field
            :label="required($t('courseManagement.lessons.name'))"
            v-model="lesson.name"
            hide-details="true"
            outlined
            dense
            placeholder=" "
          >
          </v-text-field>
        </v-col>
        <v-col cols="12">
          <v-text-field
            :label="$t('courseManagement.lessons.description')"
            v-model="lesson.description"
            outlined
            dense
            hide-details="true"
            placeholder=" "
          >
          </v-text-field>
        </v-col>
        <v-col cols="12">
          <v-text-field
            :label="$t('courseManagement.lessons.duration')"
            v-model="lesson.duration"
            outlined
            dense
            hide-details="true"
            placeholder=" "
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
import { LessonUpdateModel } from "../types";

@Component({
  components: { PopupForm, RDatePicker, RCombobox },
})
export default class CourseUpdate extends Mixins(DialogMixin, ValidationMixin) {
  @Prop({ required: true }) public idCourse!: number;
  @Prop({ required: true }) public idLesson!: number;

  controller = "courses";
  action = `${this.idCourse}/lessons/${this.idLesson}`;
  lesson: LessonUpdateModel = { name: null, description: null, duration: null };

  async created() {
    this.lesson = await apiService.get<LessonUpdateModel>(this.controller, this.action);
  }

  public get actions(): IButtonAction[] {
    return [
      {
        localeKey: "app.save",
        onAction: (action) => this.onSave(action),
      },
    ];
  }

  private async onSave(action: IButtonAction): Promise<void> {
    if (this.hasErrors) return;

    action.loading = true;

    await this.catchValidationErrorsAsync(() =>
      apiService.post(this.controller, `${this.action}/update`, this.lesson)
    );

    if (!this.hasErrors)
     this.close(true);
  }
}
</script>