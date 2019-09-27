var MessageComponent = Vue.component('MessageComponent', {
    props: ['author', 'text', 'date'],
    template: `
        <article class="message">
            <section class="author">
                {{ author }}
            </section>
            <section class="text">
                {{ text }}
            </section>
            <section class="options">
                <slot></slot>
            </section>
            <section class="date">
                {{ date }}
            </section>
            <div style="clear:both"></div>
        </article>
    `,

})

export default MessageComponent;