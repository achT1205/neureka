export default {
  methods: {
    uuid(e) {
      if (e.id) return e.id;
      const timestamp = ((new Date().getTime() / 1000) | 0).toString(16);
      const key =
        timestamp +
        "xxxxxxxxxxxxxxxx"
          .replace(/[x]/g, function() {
            return ((Math.random() * 16) | 0).toString(16);
          })
          .toLowerCase();
      this.$set(e, "id", key);
      return e.id;
    }
  }
};
