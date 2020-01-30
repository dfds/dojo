import CustomerProfile from "./module-customer";
import OrderHistory from "./module-orderhistory";
import Recommendations from "./module-recommendations";

import { loadCustomer, loadOrders, loadRecommendations } from "./data-loaders";

const customerModule = CustomerProfile.initialize("#customer-profile");
const orderHistoryModule = OrderHistory.initialize("#order-history");
const recommendationsModule = Recommendations.initialize("#recommendations");

setInterval(() => {

    loadCustomer(data => customerModule.updateCustomer(data));
    loadOrders(data => orderHistoryModule.updateOrders(data));
    loadRecommendations(data => recommendationsModule.updateProducts(data));

}, 2000);