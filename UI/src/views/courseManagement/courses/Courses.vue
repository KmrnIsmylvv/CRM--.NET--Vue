<template>
  <r-table
    :controller="controller"
    :headers="headers"
    :paging="paging"
    :state-key="stateKey"
    :toolbar="toolbar"
    :reload.sync="reloadTable"
    @rowClicked="onRowClicked"
  >
    <template #advanced-search>
      <r-advanced-search
        state-key="advancedSearch"
        :text-filter="searchFilters" 
      >
        <!-- <template #custom-autocomplete="{ field }">
          <v-select
            autocomplete
            :items="['item1', 'item2']"
            :label="field.localeKey"
            v-model="field.filter.rule.value"
          >
          </v-select>
        </template> -->
      </r-advanced-search>
    </template>

    <template #startDate="{ item }">
      {{ dateToString(item.startDate, "DD/MM/YYYY hh:mm") }}
    </template>

    <template #endDate="{ item }">
      {{ dateToString(item.endDate, "DD/MM/YYYY hh:mm") }}
    </template>

    <template #toolbar-add>
      <v-btn @click="onCourseCreate" icon v-show-for.all="[$p.canWriteCourse]">
        <v-icon>fa-plus</v-icon>
      </v-btn>
    </template>

    <template #row-actions="{ item }">
      <r-action-buttons
        :actions="tableActions"
        :context="item"
        :debounce="300"
        text
      >
        <template #toggle>
          <v-switch
            v-show-for.all="[$p.canWriteCourse]"
            :input-value="item.isEnabled"
            hide-details
          />
        </template>
      </r-action-buttons>
    </template>
  </r-table>
</template>

<script lang="ts">
import { Component, Mixins, Vue, Watch } from "vue-property-decorator";

import {
  appBarVuexNamespace,
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
  ITableToolbarFilterBy,
  ISearchFieldSelectGroupItem,
  ISearchFilter,
  SearchFieldBase,
  SearchFieldBuilder,
  RAdvancedSearch,
} from "@/container";

import Create from "./course-create.vue";
import Update from "./course-update.vue";
import { CourseUpdateModel, ICourseDetail, ICourseState } from "./types";
import CourseDetailPanel from "./course-detail.vue";
import { namespace } from "vuex-class";

const courseVuexNamespace = namespace("course");
@Component({
  components: {
    RActionButtons,
    RTable,
    RAdvancedSearch
  },
})
export default class Courses extends Mixins(DateTimeMixin) {
  controller = "courses";
  stateKey = "coursesTable";
  isPanelOpen: boolean = false;
  selectedCourse: ICourseDetail | null = null;

  @appBarVuexNamespace.Mutation
  setTitle!: (toolbarTitle: IAppBarTitleModel | null) => void;

  @courseVuexNamespace.Mutation
  setCurrentCourse!: (course: ICourseDetail | null) => void;

  created() {
    this.setTitle({
      pageContext: this.$t("courseManagement.courses").toString(),
      additionalInfo: this.$t("courseManagement").toString(),
    });
  }

  headers: ITableHeader[] = [
    {
      visible: false,
      visibilityChangeable: false,
      field: "idCourse",
    },
    {
      field: "name",
      localeKey: "courseManagement.courses.name",
    },
    {
      field: "teacherFullName",
      localeKey: "courseManagement.courses.teacher",
    },
    {
      field: "description",
      localeKey: "courseManagement.courses.description",
    },
    {
      field: "startDate",
      localeKey: "courseManagement.courses.startDate",
      format: "startDate",
    },
    {
      field: "endDate",
      localeKey: "courseManagement.courses.endDate",
      format: "endDate",
    },
    {
      field: "isEnabled",
      localeKey: "courseManagement.courses.isEnabled",
    },
    {
      format: "row-actions",
      sortable: false,
      visibilityChangeable: false,
      settingsLocaleKey: "app.actions",
      width: "60px",
    },
  ];

  paging: ITablePagingOptions = {
    hasNavigationOutside: true,
    hideNavigationIfOnePageOnly: true,
  };

  textFilterStructure: ITableFilter = {
    filters: [
      Filter.withRule("name", RuleOperator.IsLike),
      Filter.withRule("teacherFullName", RuleOperator.IsLike),
      Filter.withRule("description", RuleOperator.IsLike),
    ],
    operator: FilterOperator.Or,
  };

   searchFilters: ISearchFilter  = {
    filters: [
      Filter.withRule("name", RuleOperator.IsLike),
      Filter.withRule("teacherFullName", RuleOperator.IsLike),
      Filter.withRule("description", RuleOperator.IsLike),
    ],
    operator: FilterOperator.Or,
  };

  toolbar: ITableToolbarItem[] = [
    // {
    //   format: "search",
    //   searchProps: {
    //     filter: this.textFilterStructure,
    //     minLength: 2,
    //     textFieldProps: {
    //       outlined: true,
    //       dense: true,
    //     },
    //   },
    // },
    {
      format: "advanced-search",
      colProps:{
        cols: '*'
      }
    },
    {
      format: "filterBy",
      filterByProps: {
        icon: "fa-check",
        "state-key": "courses-status-filter",
        selected: [Filter.withRule("isEnabled", RuleOperator.IsEqual, true)],
        "filter-tag": "status-filter",
        "header-max-width": "250px",
      },
      items: [
        {
          text: "Active",
          filter: Filter.withRule("isEnabled", RuleOperator.IsEqual, true),
        },
        {
          text: "Disabled",
          filter: Filter.withRule("isEnabled", RuleOperator.IsEqual, false),
        },
      ],
    } as ITableToolbarFilterBy,
    { format: "spacer" },
    { format: "toolbar-add" },
    { format: "export" },
    { format: "settings" },
  ];

  tableActions = (context) => {
    if (this.$ps.hasAll([this.$p.canWriteCourse])) {
      return [
        {
          icon: "fa-edit",
          onAction: this.onCourseUpdate,
          disabled: !context.isEnabled,
        },
        {
          icon: "fa-info",
          onAction: this.onCourseDetail,
        },
        {
          format: "toggle",
          onAction: this.onCourseToggle,
        },
      ] as IButtonAction[];
    }

    return [
      {
        icon: "fa-info",
        onAction: this.onCourseDetail,
      },
    ] as IButtonAction[];
  };

  async onRowClicked({ item }: { item: ICourseDetail }) {
    if (
      this.selectedCourse == null ||
      this.selectedCourse.idCourse !== item.idCourse
    )
      this.selectedCourse = item;

    this.setCurrentCourse(this.selectedCourse);

    if (this.isPanelOpen) return;

    const panel = dialogService.panel({
      component: CourseDetailPanel,
      containerProps: {
        width: "50%",
        right: true,
        overlay: true,
        entranceDelay: 700,
      },
    });

    this.isPanelOpen = true;

    const result: boolean = await panel.result;

    this.isPanelOpen = false;

    this.setCurrentCourse(null);
  }

  onCourseDetail(action: IButtonAction, { idCourse }): void {
    this.$router.push({
      name: "courseManagement.courses.course",
      params: { id: idCourse.toString() },
    });
  }

  async onCourseCreate(options) {
    const saved = await dialogService.popup({
      component: Create,
      containerProps: { width: "50%" },
    }).result;

    if (saved) await this.reloadTable();
  }

  async onCourseUpdate(action: IButtonAction, { idCourse }): Promise<void> {
    action.loading = true;
    const course = await apiService.get<CourseUpdateModel>(
      this.controller,
      `${idCourse}`
    );
    action.loading = false;

    const saved = await dialogService.popup({
      component: Update,
      componentProps: { course: course },
      containerProps: { width: "50%" },
    }).result;

    if (saved) location.reload();
  }

  async reloadTable() {
    return;
  }

  async onCourseToggle(
    action: IButtonAction,
    { idCourse, isEnabled }
  ): Promise<void> {
    console.log(isEnabled);
    await apiService.post(
      this.controller,
      `${idCourse}/${isEnabled ? "disable" : "enable"}`
    );

    await this.reloadTable();
  }
}
</script>
