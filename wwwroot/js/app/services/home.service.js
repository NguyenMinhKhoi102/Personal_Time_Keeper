document.addEventListener('alpine:init', () => {
    Alpine.data("logTime", () => ({
        logTimeList: [],
        searchQuery: "",
        filteredLogTimeList: [],
        paginatedLogTimeList: [],
        activityTypeList: [],
        addPayload: {},
        editPayload: {},
        spinner1: false,
        spinner2: false,
        modalStatus: false,
        pageSize: 5,
        currentPage: 1,
        sizeList: [5, 10, 20, 50, 100],
        debounceTimeout: null,
        errors: {},
        editErrors: {},

        get totalPages() {
            const totalPageTemp = Math.ceil(this.filteredLogTimeList.length / this.pageSize);
            return totalPageTemp > 0 ? totalPageTemp : 1;
        },

        get startPage() {
            return (this.currentPage - 1) * this.pageSize;
        },

        get endPage() {
            const endPageTemp = Math.ceil(this.startPage + this.pageSize);
            return endPageTemp > this.filteredLogTimeList.length ? this.filteredLogTimeList.length : endPageTemp;
        },

        filterLogTimeList(query) {
            if (query === '' || query === undefined || query === null) {
                this.filteredLogTimeList = this.logTimeList;
            } else {
                this.filteredLogTimeList = this.logTimeList.filter(log => {
                    return Object.values(log).some(value => {
                        return String(value).toLowerCase().includes(query.toLowerCase());
                    });
                });
            }
            this.currentPage = 1;
            this.paginationLogTimeList();
        },

        debounceFilter(query) {
            clearTimeout(this.debounceTimeout);
            this.debounceTimeout = setTimeout(() => {
                this.filterLogTimeList(query);
            }, 300);
        },

        paginationLogTimeList() {
            this.paginatedLogTimeList = this.filteredLogTimeList.slice(this.startPage, this.endPage);
        },

        setCurrentPage(index) {
            this.currentPage = index;
            this.paginationLogTimeList();
        },

        nextPage() {
            if (this.currentPage < this.totalPages) {
                this.currentPage++;
                this.paginationLogTimeList();
            }
        },

        prevPage() {
            if (this.currentPage > 1) {
                this.currentPage--;
                this.paginationLogTimeList();
            }
        },

        async fetchLogTimeList() {
            try {
                this.spinner1 = true;
                const rs = await axios.get("/api/logTime");
                this.logTimeList = rs.data;
                this.paginatedLogTimeList = rs.data;
                this.filterLogTimeList();
            } catch (errors) {
                console.error(errors);
            } finally {
                setTimeout(() => {
                    this.spinner1 = false;
                }, 500);
            }
        },

        async fetchActivityTypeList() {
            try {
                this.spinner2 = true;
                const rs = await axios.get("/api/activityType");
                this.activityTypeList = rs.data;
                this.addPayload.activityTypeId = rs.data[0].id;
            } catch (errors) {
                console.error(errors);
            } finally {
                setTimeout(() => {
                    this.spinner2 = false;
                }, 100);
            }
        },

        logTimeValidation() {
            this.errors = {};

            if (!this.addPayload.logDate)
                this.errors.logDate = "Log date is required";

            if (!this.addPayload.activityTypeId)
                this.errors.activityType = "Activity type is required";

            if (!this.addPayload.duration)
                this.errors.duration = "Duration is required";
            else if (this.addPayload.duration < 0.25)
                this.errors.duration = "Duration must be greater than or equal to 0.25";
            else if (this.addPayload.duration > 8)
                this.errors.duration = "Duration must be less than or equal to 8";

            return Object.keys(this.errors).length === 0;
        },

        editLogTimeValidation() {
            this.editErrors = {};

            if (!this.editPayload.logDate)
                this.editErrors.logDate = "Log date is required";

            if (!this.editPayload.activityTypeId)
                this.editErrors.activityType = "Activity type is required";

            if (!this.editPayload.duration)
                this.editErrors.duration = "Duration is required";
            else if (this.editPayload.duration < 0.25)
                this.editErrors.duration = "Duration must be greater than or equal to 0.25";
            else if (this.editPayload.duration > 8)
                this.editErrors.duration = "Duration must be less than or equal to 8";

            return Object.keys(this.editErrors).length === 0;
        },

        async onSubmitAddLogTime() {
            try {
                if (this.logTimeValidation()) {
                    const rs = await axios.post("api/logTime", this.addPayload);
                    alert(rs.data.message);
                    this.onRefresh();
                    this.onRefreshAddForm();
                }
            } catch (errors) {
                console.error(errors);
            }
        },

        async onSubmitEditLogTime() {
            try {
                if (this.editLogTimeValidation()) {
                    const rs = await axios.put(`api/logTime/${this.editPayload.id}`, this.editPayload);
                    this.editErrors = {};
                    this.modalStatus = false;
                    alert(rs.data.message);
                    this.onRefresh();
                }
            } catch (errors) {
                console.error(errors);
            }
        },


        async onDeleteLogTime(id) {
            try {
                if (confirm("Do you want to delete this log time")) {
                    const rs = await axios.delete(`api/logTime/${id}`);
                    alert(rs.data.message);
                    this.onRefresh();
                }
            } catch (errors) {
                console.error(errors);
            }
        },

        onRefreshAddForm() {
            this.errors = {};
            this.addPayload = {};
            this.fetchActivityTypeList();
        },

        onRefresh() {
            this.searchQuery = "";
            this.fetchLogTimeList();
        },

        openModal(payload) {
            this.editPayload = {
                id: payload.id,
                logDate: payload.logDate,
                activityTypeId: payload.activityTypeId,
                duration: payload.duration,
                description: payload.description,
            };
            this.editErrors = {};
            this.modalStatus = true;
        },

        onInit() {
            this.fetchActivityTypeList();
            this.fetchLogTimeList();
            this.$watch("searchQuery", () => this.debounceFilter(this.searchQuery));
            this.$watch("pageSize", () => {
                this.currentPage = 1;
                this.paginationLogTimeList();
            });
        },
    }));
});