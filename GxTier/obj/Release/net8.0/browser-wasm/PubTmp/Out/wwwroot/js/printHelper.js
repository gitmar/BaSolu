/*
window.printCardAsPDF = function (elementId, filename) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();
    alert('Printing...');
    const element = document.getElementById(elementId);
    if (!element) return;

    doc.html(element, {
        callback: function (pdf) {
            pdf.save(filename || "print.pdf");
        },
        x: 10,
        y: 10
    });
};
*/
window.printForm = (selector) => {
    const printContents = document.querySelector(selector);
    if (!printContents) {
        console.error("Element not found:", selector);
        return;
    }

    const originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents.outerHTML;

    window.print();

    document.body.innerHTML = originalContents;
    location.reload(); // reload to restore Blazor interactivity
};

//window.printCardAsPDF = function (elementId, filename) {
//    const { jsPDF } = window.jspdf;
//    const doc = new jsPDF();

//    const element = document.getElementById(elementId);
//    if (!element) return;

//    doc.html(element, {
//        callback: function (pdf) {
//            pdf.save(filename || "print.pdf");
//        },
//        x: 10,
//        y: 10
//    });
//};

//window.generatePdf = (htmlId) => {
//    const { jsPDF } = window.jspdf;
//    const doc = new jsPDF("p", "pt", "a4");

//    const element = document.getElementById(htmlId);
//    if (!element) {
//        console.error("Element not found: " + htmlId);
//        return;
//    }

//    doc.html(element, {
//        callback: function (doc) {
//            doc.save("form.pdf");
//        },
//        margin: [20, 20, 20, 20],
//        autoPaging: "text",
//        x: 10,
//        y: 10,
//        html2canvas: {
//            scale: 0.8, // reduce scale if content is cut
//            logging: true
//        }
//    });
//};

function exportPdfAfterRender(elementId) {
    requestAnimationFrame(() => {
        const el = document.getElementById(elementId);
        if (el) {
            const pdf = new jsPDF();
            pdf.html(el, {
                callback: function (pdf) {
                    pdf.save('document.pdf');
                }
            });
        }
    });
}