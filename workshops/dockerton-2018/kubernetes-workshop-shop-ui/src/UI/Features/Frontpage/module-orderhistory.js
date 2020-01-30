import Vue from "vue";
import "./order-history.scss";

function initializeVueApp(elementId) {
    return new Vue({
        el: elementId,
        data: {
            orders: []
        },
        computed: {
            hasOrders: function () {
                return this.orders != null && this.orders.length > 0;
            }
        }
    });
}

export default class OrderHistory {

    constructor(vueApp) {
        this.app = vueApp;

        // this bindings
        this.updateOrders = this.updateOrders.bind(this);
    }

    updateOrders(newOrders) {
        this.app.orders = newOrders;
    }

    static initialize(elementId) {
        const app = initializeVueApp(elementId);
        return new OrderHistory(app);
    }
}