import Vue from 'vue';
import Permissions from './permissions.designer';

const permissions = new Permissions();
Vue.prototype.$p = permissions;

import { configuration } from '@/configuration';
import authFlow from '@/auth/mixins/authFlow';
import WinAuthFlowMixin from '@/auth/mixins/winAuthFlow';
import JwtAuthFlowMixin from '@/auth/mixins/jwtAuthFlow';

let AuthFlow = authFlow;
switch (configuration.authFlowType) {
	case 'JWT':
		AuthFlow = JwtAuthFlowMixin;
		break;
	case 'Windows':
		AuthFlow = WinAuthFlowMixin;
		break;
	default:
		break;
}

export { AuthFlow };
