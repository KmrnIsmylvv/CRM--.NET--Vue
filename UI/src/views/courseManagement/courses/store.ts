import {ICourseState, ICourseDetail } from './types';

const state = {
	course: null
} as ICourseState;

const mutations = {
	setCurrentCourse(courseState: ICourseState, course: ICourseDetail | null) {
		courseState.course = course;
	}
};

export const CourseVuexModule = {
    state,
    mutations,
    namespaced: true
}