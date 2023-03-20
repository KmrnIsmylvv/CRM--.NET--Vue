<template>
  <panel-form :title="$t('courseManagement.courseDetail')">
    <v-container>
      <v-row>
        <v-col cols="12">
          <v-text-field
            :label="$t('courseManagement.courses.name')"
            v-model="course.name"
            :hide-details="true"
            outlined
            dense
            readonly
          >
          </v-text-field>
        </v-col>
        <v-col cols="12">
          <v-text-field
            :label="$t('courseManagement.courses.description')"
            v-model="course.description"
            outlined
            dense
            :hide-details="true"
            readonly
          >
          </v-text-field>
        </v-col>
        <v-col cols="12">
          <r-date-picker
            :label="$t('courseManagement.courses.startDate')"
            open-on-icon-click
            v-model="course.startDate"
            :hide-details="true"
            outlined
            dense
            readonly
          >
          </r-date-picker>
        </v-col>
        <v-col cols="12">
          <r-date-picker
            :label="$t('courseManagement.courses.endDate')"
            open-on-icon-click
            v-model="course.endDate"
            :hide-details="true"
            outlined
            dense
            readonly
          >
          </r-date-picker>
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
} from "@/container";
import { PanelForm } from "@/layouts/default/containers";
import { namespace } from "vuex-class";
import { ICourseDetail } from "./types";

const courseVuexNamespace = namespace("course");

@Component({
  components: { PanelForm, RDatePicker },
})
export default class CourseDetail extends Mixins(DialogMixin) {
  @courseVuexNamespace.State course!: ICourseDetail;
}
</script>



