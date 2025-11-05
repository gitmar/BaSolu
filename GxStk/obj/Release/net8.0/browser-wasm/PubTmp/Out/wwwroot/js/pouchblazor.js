window.pouchdbBlazor = {
    createDb: function (dbName) {
        window._pdb = new PouchDB(dbName);
    },
    putDoc: async function (doc) {
        return await window._pdb.put(doc);
    },
    getDoc: async function (id) {
        return await window._pdb.get(id);
    },
    allDocs: async function () {
        return await window._pdb.allDocs({ include_docs: true });
    }
    // Add more as needed (remove, sync, etc.)
};