var PostComponent = Vue.component('PostComponent', {
    data: function () {
        return {
            isVisible: false,
        }
    },
    methods: {
        toggle() {
            this.isVisible = !this.isVisible
        }
    },
    props: ['author', 'text', 'logged'],
    template: `
        <article class="post" v-if="!isVisible">
            <section class="author">
                Autor: {{ author }}
            </section>
            <section class="options" v-if="logged">
                <button v-on:click="toggle">Edytuj</button>
                <slot name="delete"></slot>
            </section>
            <section class="text">
                {{ text }}
            </section>
        </article>
        <article class="edit" v-else-if="isVisible">
            <slot name="edit"></slot>
        </article>
    `,

})

export default PostComponent;