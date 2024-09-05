// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    fetch('/Products/OrderListNum', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    })
        .then(response => response.json())
        .then(result => {
            console.log('成功:', result);
            
            var newDiv = $('<div>', {
                text: result,
                css: {
                    'background-color': '#5955C4',
                    'color': '#fff',
                    'display': 'flex',
                    'border-radius': '50%',
                    'width': '16px',
                    'height': '16px',
                    'justify-content': 'center',
                    'align-items': 'center',
                    'font-size': '11px',
                    'position': 'absolute',
                    'right': '0',
                    'top': '-4px',
                }
            });

            // 选择目标元素
            var targetElement = $('input.order-btn');

            // 在目标元素前插入新的 div
            newDiv.insertBefore(targetElement);
        })
        .catch(error => {
            console.error('錯誤:', error);
        });
});