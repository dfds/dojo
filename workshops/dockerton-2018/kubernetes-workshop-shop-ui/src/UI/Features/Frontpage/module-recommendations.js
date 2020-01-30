import Vue from "vue";
import "./recommendations.scss";

function initializeVueApp(elementId) {
    return new Vue({
        el: elementId,
        data: {
            products: [],
            glide: null
        },
        computed: {
            hasRecommendations: function () {
                return this.products != null && this.products.length > 0;
            }
        }
    });
}

export default class Recommendations {

    constructor(vueApp) {
        this.app = vueApp;

        // this bindings
        this.updateProducts = this.updateProducts.bind(this);
        this.addProduct = this.addProduct.bind(this);
    }

    updateProducts(newProducts) {
        this.app.products = newProducts;
   }

    addProduct(newProduct) {
        this.app.products.push(newProduct);
    }

    static initialize(elementId) {
        const app = initializeVueApp(elementId);
        return new Recommendations(app);
    }
}