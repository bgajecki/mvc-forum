const addTopic = new Vue({
    el: '#addTopic',
    data: {
        isVisible: false,
        text: '↓  Dodaj temat  ↓',
        
    },
    methods: {
        toggle() {
            this.isVisible = !this.isVisible
            this.isVisible ? this.text = '↑  Zwiń  ↑' : this.text = '↓  Dodaj temat  ↓'
        }
    },
})