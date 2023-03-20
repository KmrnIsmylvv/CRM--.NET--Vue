import Permissions from './permissions.designer';
import { PermissionsService } from '@round/authorization';

declare module 'vue/types/vue' {
	interface Vue {
		$p: Permissions;
		$ps: PermissionsService;
	}
	interface VueConstructor {
		$p: Permissions;
		$ps: PermissionsService;
	}
}
