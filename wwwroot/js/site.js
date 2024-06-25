document.addEventListener('alpine:init', () => {
    Alpine.store('user', {
        info: {},

        async fetchInfoUser() {
            try {
                const rs = await axios.get("/api/account/info");
                this.info = rs.data;
            } catch (errors) {
                console.error(errors);
            }
        },
    })
})