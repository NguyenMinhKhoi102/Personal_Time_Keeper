document.addEventListener('alpine:init', () => {
    Alpine.data('login', () => {
        return {
            payload: {
                email: "",
                password: "",
            },
            message: "",

            async submitLogin() {
                try {
                    const rs = await axios.post("/api/account/login", this.payload);
                    window.location.href = "/";
                } catch (errors) {
                    this.message = "Email or password was incorrect";
                    console.error(errors);
                }
            },
        }
    });
    Alpine.data('accountEdit', () => {
        return {
            payload: {
                email: "",
                fullName: "",
                phone: "",
            },
            message: "",
            spinner: true,

            async fetchData() {
                try {
                    this.spinner = true;
                    const rs = (await axios.get("/api/account/info")).data;
                    this.payload = { ...this.payload, ...rs };
                } catch (errors) {
                    console.error(errors);
                } finally {
                    this.spinner = false;
                }
            },

            async submitForm() {
                try {
                    this.spinner = true;
                    const rs = await axios.post("/api/account/edit", this.payload);
                } catch (errors) {
                    console.error(errors);
                } finally {
                    this.spinner = false;
                }
            },
        }
    });
});
