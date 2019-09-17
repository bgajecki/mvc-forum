const addTopic = new Vue({
    el: '#addTopic',
    data: {
        isVisible: false,
        text: '↓  Dodaj temat  ↓'
    },
    methods: {
        toggle: function() {
            this.isVisible = !this.isVisible
            this.isVisible ? this.text = '↑  Zwiń  ↑' : this.text = '↓  Dodaj temat  ↓'
        }
    }
})