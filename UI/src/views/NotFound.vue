<template>
	<span>Not found</span>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';

@Component
export default class NotFound extends Vue {

}
</script>

<route>
{
	"layout": "none",
	"includeInDrawer": false,
	"path": "/404"
}
</route>
