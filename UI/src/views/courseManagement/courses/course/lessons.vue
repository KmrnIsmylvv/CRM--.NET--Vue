<template>
  <r-table
    :controller="controller"
    :action="action"
    :headers="headers"
    :paging="paging"
    :state-key="stateKey"
    :toolbar="toolbar"
    :reload.sync="reloadTable"
    auto-size
  >
    <template #toolbar-add>
      <v-btn @click="onLessonCreate" icon v-show-for.all="[$p.canWriteCourse]">
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
      </r-action-buttons>
    </template>
  </r-table>
</template>

<script lang="ts">
import { Component, Mixins, Prop, Vue, Watch } from "vue-property-decorator";

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
  ITableToolbarFilterBy,
} from "@/container";
import Create from "./lesson-create.vue";
import Update from "./lesson-update.vue";
import { LessonUpdateModel } from "../types";

@Component({
  components: {
    RActionButtons,
    RTable,
  },
})
export default class Lessons extends Vue {
  @Prop({ required: true }) public idCourse!: number;

  controller = "courses";
  action = `${this.idCourse}/lessons`;
  stateKey = "lessonsTable";

  headers: ITableHeader[] = [
    {
      visible: false,
      visibilityChangeable: false,
      field: "idCourse",
    },
    {
      visible: false,
      visibilityChangeable: false,
      field: "idLesson",
    },
    {
      field: "name",
      localeKey: "courseManagement.lessons.name",
    },
    {
      field: "description",
      localeKey: "courseManagement.lessons.description",
    },
    {
      field: "duration",
      localeKey: "courseManagement.lessons.duration",
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
      Filter.withRule("description", RuleOperator.IsLike),
      Filter.withRule("duration", RuleOperator.IsLike),
    ],
    operator: FilterOperator.Or,
  };

  toolbar: ITableToolbarItem[] = [
    {
      format: "search",
      searchProps: {
        filter: this.textFilterStructure,
        minLength: 2,
        textFieldProps: {
          outlined: true,
          dense: true,
        },
      },
    },
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
          onAction: this.onLessonUpdate,
        },
        {
          icon: "fa-trash",
          onAction: this.onLessonDelete,
        },
      ] as IButtonAction[];
    }
  };

  async onLessonCreate() {
    const saved = await dialogService.popup({
      component: Create,
      componentProps: { idCourse: this.idCourse },
      containerProps: { width: "50%" },
    }).result;
    if (saved) await this.reloadTable();
  }

  async onLessonUpdate(action: IButtonAction, { idLesson }): Promise<void> {
    const saved = await dialogService.popup({
      component: Update,
      componentProps: { idCourse: this.idCourse, idLesson: idLesson },
      containerProps: { width: "50%" },
    }).result;
    if (saved) location.reload();
  }

  async reloadTable() {
    return;
  }

  async onLessonDelete(action: IButtonAction, { idLesson }): Promise<void> {
    const confirm = await dialogService.alert("Warning!", "Are you sure?")
      .result;

    if (confirm) {
      await apiService.delete(this.controller, `${this.action}/${idLesson}`);

      await this.reloadTable();
    }
  }
}
</script>
