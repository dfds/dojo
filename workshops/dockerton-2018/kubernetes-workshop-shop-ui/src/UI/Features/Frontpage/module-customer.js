import Vue from "vue";
import "./customer-profile.scss";

function initializeVueApp(elementId) {
    return new Vue({
        el: elementId,
        data: {
            customer: null
        },
        computed: {
            hasCustomer: function () {
                return this.customer != null;
            },
            getPictureUrl: function () {
                if (!this.customer.picture) {
                    return "https://www.ptibogxiv.net/wp-content/plugins/doliconnect/images/default.jpg";
                }

                return this.customer.picture;
            },
            getAddress: function () {
                if (!this.customer.address) {
                    return "";
                }
                
                return this.customer.address.join("<br />");
            }
        }
    });
}

export default class CustomerProfile {

    constructor(vueApp) {
        this.app = vueApp;

        // this bindings
        this.updateCustomer = this.updateCustomer.bind(this);
    }

    updateCustomer(newCustomer) {
        this.app.customer = newCustomer;
    }

    static initialize(elementId) {
        const app = initializeVueApp(elementId);
        return new CustomerProfile(app);
    }
}