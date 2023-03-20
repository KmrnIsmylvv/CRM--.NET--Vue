const routes = [
	{
		name: 'home',
		path: '/',
		component: () => import('@/views/Home.vue'),
		meta: {
			icon: 'fa-home'
		}
	},
	{
		name: 'forbidden',
		path: '/403',
		component: () => import('@/views/Forbidden.vue'),
		meta: {
			layout: 'none',
			includeInDrawer: false
		}
	},
	{
		name: 'notFound',
		path: '/404',
		component: () => import('@/views/NotFound.vue'),
		meta: {
			layout: 'none',
			includeInDrawer: false
		}
	},
	{
		name: 'courseManagement',
		path: '/courseManagement',
		redirect: { name: 'courseManagement.courses' },
		meta: {
			icon: 'fa-book',
			includeInDrawer: true
		}
	},
	{
		name: 'courseManagement.courses',
		path: '/courseManagement/courses',
		component: () => import('@/views/courseManagement/courses/Courses.vue')
	},
	{
		name: 'courseManagement.courses.course',
		path: '/courseManagement/courses/:id',
		component: () => import('@/views/courseManagement/courses/course/Course{id}.vue'),
		meta: {
			includeInDrawer: false
		}
	},
	{
		name: 'courseManagement.exams',
		path: '/courseManagement/exams',
		component: () => import('@/views/courseManagement/exams/Exams.vue')
	},
	{
		name: 'courseManagement.exams.exam',
		path: '/courseManagement/exams/:id',
		component: () => import('@/views/courseManagement/exams/exam/Exam{id}.vue'),
		meta: {
			includeInDrawer: false
		}
	},
	{
		name: 'hangfire',
		path: '/hangfire',
		component: () => import('@/views/Hangfire.vue'),
		meta: {
			visibleTo: [
				'HangfireDashboard_Reader'
			],
			includeInDrawer: false
		}
	},
	{
		name: 'login',
		path: '/login',
		component: () => import('@/views/Login.vue'),
		meta: {
			layout: 'none',
			includeInDrawer: false
		}
	},
	{
		name: 'queryViewer',
		path: '/queryViewer',
		component: () => import('@/views/QueryViewer.vue'),
		meta: {
			layout: 'none',
			includeInDrawer: false,
			visibleTo: [
				'Round_CanProfile'
			]
		}
	},
	{
		name: 'systemManagement',
		path: '/systemManagement',
		redirect: { name: 'systemManagement.users' },
		meta: {
			icon: 'fa-user',
			includeInDrawer: true
		}
	},
	{
		name: 'systemManagement.groups',
		path: '/systemManagement/groups',
		component: () => import('@/views/systemManagement/groups/Groups.vue')
	},
	{
		name: 'systemManagement.users',
		path: '/systemManagement/users',
		component: () => import('@/views/systemManagement/users/Users.vue')
	}
];

export default routes;
