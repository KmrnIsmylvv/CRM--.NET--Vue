<template>
  <v-app>
    <v-content>
			<RQueryViewer/>
		</v-content>
  </v-app>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import { RQueryViewer } from '@/container';

@Component({
	components: { RQueryViewer }
})
export default class QueryViewer extends Vue {

}
</script>

<route>
{
	"layout": "none",
	"includeInDrawer": false,
	"visibleTo": [ 'Round_CanProfile' ]
}
</route>
