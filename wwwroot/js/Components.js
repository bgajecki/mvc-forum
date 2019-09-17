import TopicComponent from './TopicComponent.js';
import PostComponent from './PostComponent.js';

new Vue({
    el: '#forum',
    components: {
        'topic': TopicComponent,
        'post': PostComponent
    }
})