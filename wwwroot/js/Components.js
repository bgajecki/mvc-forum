import TopicComponent from './TopicComponent.js';
import PostComponent from './PostComponent.js';
import MessageComponent from './MessageComponent.js';

new Vue({
    el: '#forum',
    components: {
        'topic': TopicComponent,
        'post': PostComponent,
        'message': MessageComponent
    }
})