import createApiClient from "./api.service";

class AccountService {
    constructor(baseUrl = "/api/account") {
        this.api = createApiClient(baseUrl);
    }

    async login(data) {
        return (await this.api.post("/login"), data).data;
    }
}

export default new AccountService;