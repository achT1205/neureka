export default {
    methods: {
        handleClone(item) {
            let cloneMe = JSON.parse(JSON.stringify(item));
            this.$delete(cloneMe, "id");
            return cloneMe;
        },
    }
}