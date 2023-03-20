<template>
  <v-card class="popup-form popup-container">
    <v-card-title class="primary white--text">
			<v-container class="pa-0">
				<v-row no-gutters>
					<v-col align-self="center" class="text-truncate">
						<span v-text="title" :class="titleClass"/>
					</v-col>
					<v-col cols="auto">
						<r-action-buttons 
							:debounce="300"
							:actions="topActions" 
							:commonOptions="topActionsCommonOptions"
							:containerOptions="topActionsContainerOptions"/>
					</v-col>							
				</v-row>
			</v-container>
    </v-card-title>
		<v-card-text class="grow popup-content pt-4">
			<slot />
		</v-card-text>
		<v-divider></v-divider>
    <v-card-actions>
			<v-container class="pa-0">
				<r-action-buttons 
					:debounce="300"
					:actions="actions"
					:commonOptions="footerActionsCommonOptions"
					:containerOptions="footerActionsContainerOptions"/>
			</v-container>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">

import { Vue, Component, Prop } from 'vue-property-decorator';
import { RActions, DialogMixin, IAction, RActionButtons, IButtonAction, IActionsContainer } from '@/container';

@Component({
	components: { RActionButtons }
})
export default class PopupForm extends Vue {
	@Prop({ default: 'headline' }) public titleClass!: string;
	@Prop({ default: [] }) public title!: string;
	@Prop({ default: () => [] }) public actions!: IButtonAction[];

	topActionsContainerOptions: IActionsContainer = {
		props: {
			class: 'dialog-actions',
			'justify-end': true
		}
	};

	topActionsCommonOptions: IButtonAction = {
		props: {
			class: 'dialog-action',
			color: 'white'
		}
	};

	get topActions(): IButtonAction[] {
		const actions: IButtonAction[] = [];
		actions.push({
			icon: 'fa-times',
			onAction: this.onClose,
			props: {
				text: true,
				icon: true
			}
		});
		return actions;
	}

	footerActionsContainerOptions: IActionsContainer = {
		props: {
			class: 'dialog-actions',
			'justify': 'end'
		}
	};

	footerActionsCommonOptions: IButtonAction = {
		props: {
			class: 'dialog-action',
			outlined: true,
			large: true
		}
	};

	public onClose() {
		this.$parent.$emit('close', false);
	}
}

</script>

<style lang="scss">

.popup-container {
	overflow: hidden;
	display: flex;
	flex-direction: column;
}

.popup-content {
	height: 50%;
}

</style>

