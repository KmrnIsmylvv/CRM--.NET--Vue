<template>
	<div class="home">
		hello
	</div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import { appBarVuexNamespace, IAppBarTitleModel, langVuexNamespace } from '@/container';

@Component
export default class Home extends Vue {

	@appBarVuexNamespace.Mutation
	setTitle!: (toolbarTitle: IAppBarTitleModel | null) => void;

	@langVuexNamespace.State
	loadingLocaleSuccess!: boolean;

	mounted() {
		this.updateTitle();
	}

	@Watch('loadingLocaleSuccess')
	updateTitle() {
		if (this.loadingLocaleSuccess)
			this.setTitle({
				pageContext: this.$t('home').toString()
			});
	}
}
</script>

<route>
{
	"path": "/",
	"icon": "fa-home"
}
</route>
