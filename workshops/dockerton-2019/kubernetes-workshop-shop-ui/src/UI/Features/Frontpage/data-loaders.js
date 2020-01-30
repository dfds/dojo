import jq from "jquery";

function getUserId() {
    const temp = new URLSearchParams(window.location.search);
    const id = temp.get("id");
    return id || "1";
}

export function loadCustomer(callback) {
    const id = getUserId();
    jq.getJSON(`${window.baseUrl}/api/myaccount/${id}/profile`)
        .then(data => {
            const address = [ data.address.streetname, data.address.zipcode, data.address.city ];
            const result = Object.assign({ picture: data.imageUrl }, data);
            result.address = address;

            callback(result);
        })
        .catch(err => callback(null));
};

export function loadOrders(callback) {
    const id = getUserId();
    jq.getJSON(`${window.baseUrl}/api/myaccount/${id}/orders`)
        .then(data => {
            const temp = data.items.map(order => {
                let total = 0;
                order.lines.forEach(x => total += (x.price*x.quantity));

                return {
                    id: order.number,
                    date: order.submittet,
                    lines: order.lines.map(line => {
                        return {
                            description: line.description,
                            imageUrl: line.imageUrl,
                            quantity: line.quantity,
                            price: line.price
                        };
                    }),
                    total: total
                };
            });

            callback(temp);
        })
        .catch(err => callback(null));
};

export function loadRecommendations(callback) {
    const id = getUserId();
    jq.getJSON(`${window.baseUrl}/api/myaccount/${id}/recommendations`)
        .then(data => callback(data.products))
        .catch(err => callback(null));
};