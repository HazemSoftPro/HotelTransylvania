import { defineStore } from 'pinia'

export const useSidebarStore = defineStore({
	id: 'SidebarStore',
	state: () => ({
		isCollapsed: false,
	}),
	actions: {
		toggleSidebar() {
			this.isCollapsed = !this.isCollapsed
		},
	},
})
