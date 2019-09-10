const addPost = new Vue({
    el: '#addPost',
    data: {
        isVisible: false,
        text: '↓  Dodaj post  ↓',
    },
    methods: {
        toggle() {
            this.isVisible = !this.isVisible,
            this.isVisible ? this.text = '↑  Zwiń  ↑' : this.text = '↓  Dodaj post  ↓'
        }
    },
})