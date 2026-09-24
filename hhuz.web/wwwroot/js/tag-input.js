document.addEventListener('DOMContentLoaded', () => {
    const input = document.getElementById('tags-input');
    if (!input) return;

    const tagify = new Tagify(input, {
        whitelist: [],
        dropdown: { enabled: 1, maxItems: 10 },
        // Formaga oddiy "a, b, c" matni ketadi, backend o'zgarmaydi
        originalInputValueFormat: values => values.map(v => v.value).join(', ')
    });

    let controller;

    tagify.on('input', e => {
        const q = e.detail.value;
        if (!q) return;

        controller?.abort();              // oldingi so'rovni bekor qilamiz
        controller = new AbortController();

        tagify.whitelist = null;
        tagify.loading(true);

        fetch(`/tags/search?q=${encodeURIComponent(q)}`, { signal: controller.signal })
            .then(r => r.json())
            .then(list => {
                tagify.whitelist = list;
                tagify.loading(false).dropdown.show(q);
            })
            .catch(() => tagify.loading(false));
    });
});