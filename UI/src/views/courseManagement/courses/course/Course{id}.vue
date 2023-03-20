

<template>
  <v-container fluid>
    <v-row>
      <v-col cols="12">
        <v-text-field
          v-model="course.name"
          :label="$t('courseManagement.courses.name')"
          hide-details="true"
          dense
          outlined
          readonly
        >
        </v-text-field>
      </v-col>

      <v-col>
        <v-text-field
          v-model="course.teacherFullname"
          :label="$t('courseManagement.courses.teacher')"
          hide-details="true"
          dense
          outlined
          readonly
        >
        </v-text-field>
      </v-col>

      <v-col cols="12">
        <v-text-field
          v-model="course.description"
          :label="$t('courseManagement.courses.description')"
          hide-details="true"
          dense
          outlined
          readonly
        >
        </v-text-field>
      </v-col>

      <v-col cols="6">
        <r-date-picker
          :label="$t('courseManagement.courses.startDate')"
          v-model="course.startDate"
          picker-type="both"
          :hide-details="true"
          dense
          outlined
          readonly
        >
        </r-date-picker>
      </v-col>

      <v-col cols="6">
        <r-date-picker
          :label="$t('courseManagement.courses.endDate')"
          v-model="course.endDate"
          picker-type="both"
          :hide-details="true"
          dense
          outlined
          readonly
        >
        </r-date-picker>
      </v-col>

      <v-col cols="12">
        <v-checkbox
          v-model="course.isEnabled"
          :label="$t('courseManagement.courses.isEnabled')"
          hide-details="true"
          dense
          outlined
          readonly
        >
        </v-checkbox>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-card class="flex-grow-0">
          <v-card-title class="blue"> Lessons </v-card-title>
          <Lessons :idCourse="idCourse" />
        </v-card>
      </v-col>

      <v-col cols="12"> </v-col>
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
} from "@/container";
import { CourseDetail } from "../types";
import Lessons from "./lessons.vue";

@Component({
  components: {
    RDatePicker,
    Lessons,
  },
})
export default class Course extends Mixins(DateTimeMixin) {
  idCourse = this.$route.params.id;
  course: CourseDetail = {
    teacherFullname: null,
    name: null,
    description: null,
    startDate: null,
    endDate: null,
    isEnabled: false,
  };

  @appBarVuexNamespace.Mutation
  setTitle!: (toolbarTitle: IAppBarTitleModel | null) => void;

  @appBarVuexNamespace.Mutation
  setNavigationButton!: (navigationButton: INavigationButtonModel) => void;

  @appBarVuexNamespace.Mutation
  clearNavigationButton!: () => void;

  async created() {
    this.course = await apiService.get<CourseDetail>("courses", this.idCourse);

    this.setTitle({
      pageContext: `${this.$t("courseManagement.course").toString()} ${
        this.idCourse
      }`,
      additionalInfo: this.$t("courseManagement").toString(),
    });

    this.setNavigationButton({
      routeName: "courseManagement.courses",
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
<style>
.my-container {
  max-height: 400px; /* set a maximum height for the container */
  overflow-y: auto; /* enable vertical scrolling */
}
</style>
