
$(function () {
    // Sayfa yüklendiğinde ilk bölgeyi aktif yap
    var firstRegion = $('.regions-list .region-box').first();
    if (firstRegion.length > 0) {
        firstRegion.addClass('active');
        loadTablesForArea(firstRegion.data('id'));
    }

    // Bölgeye tıklama olayını yakala
    $('.regions-list').on('click', '.region-box', function () {
        var areaId = $(this).data('id');

        // Tıklanan bölgeyi aktif yap ve diğerlerini pasif yap
        $('.region-box').removeClass('active');
        $(this).addClass('active');

        // Masaları yükle
        loadTablesForArea(areaId);
    });

    // Masaları yükleme fonksiyonu
    function loadTablesForArea(areaId) {
        // Önceki masaları temizle
        $('.drop-area').empty();

        // AJAX ile bölgeye ait masaları getir
        $.ajax({
            method: 'GET',
            url: '/Table/GetTablesByArea', // Controller'daki metodun URL'si
            data: { areaId: areaId }, // Gönderilecek veri
            success: function (tables) {
                // Masaları ekranda yerleştir
                tables.forEach(function (table) {
                    var tableImage;

                    // Shape değerine göre görsel belirle
                    if (table.shape === 'Kare') {
                        tableImage = '/img/kare-masa.png';
                    } else if (table.shape === 'Yuvarlak') {
                        tableImage = '/img/yuvarlak-masa.png';
                    }

                    var tableElement = $('<div class="draggable-box" data-id="' + table.tableId + '"></div>');
                    tableElement.css({
                        left: table.posX + 'px',
                        top: table.posY + 'px'
                    });
                    tableElement.html(`
                                 <div class="table-content">
                                    <img src="${tableImage}" alt="Masa" class="table-image">
                                    <span class="table-text">${table.tableName}</span>
                                    <div class="edit-icon">
                                        <i class="fa fa-edit btn-editTable"></i>
                                    </div>
                                 </div>
                            `);
                    $('.drop-area').append(tableElement);
                });

                // Masaları sürüklenebilir hale getir
                $(".draggable-box").draggable({
                    containment: ".drop-area",
                    stop: function (event, ui) {
                        var posX = ui.position.left;
                        var posY = ui.position.top;
                        var tableId = $(this).data('id');

                        $.ajax({
                            method: 'POST',
                            url: '/Table/UpdateTablePosition',
                            contentType: 'application/json',
                            data: JSON.stringify({
                                tableId: tableId,
                                posX: posX,
                                posY: posY
                            }),
                            success: function (response) {
                                console.log('Pozisyon güncellendi.');
                            },
                            error: function (xhr, status, error) {
                                console.error('Pozisyon güncellenirken hata oluştu:', error);
                            }
                        });
                    }
                });
            },
            error: function (xhr, status, error) {
                console.error('Masalar yüklenirken hata oluştu:', error);
            }
        });
    }
});

$(document).ready(function () {
    // Butona tıklama olayı
    $('.btn-addArea').on('click', function () {

        $('#listAreaModal').modal('hide');
        // Modalı aç
        $('#addAreaModal').modal('show');
    });
});

$(document).ready(function () {
    // Kaydet butonuna tıklama olayını yakalayın
    $('#addAreaModal .btn-primary').click(function () {
        var regionName = $('#regionName').val();

        // AJAX ile veriyi gönderin
        $.ajax({
            url: '/Table/AddRegion', // Controller'daki Action method
            type: 'POST',
            data: { regionName: regionName }, // Gönderilen veri
            success: function (response) {
                if (response.success) {
                    // Başarılı kayıttan sonra modalı kapat
                    $('#addAreaModal').modal('hide');

                    // Bölge listesini güncelle veya başka bir işlem yap
                    toastr.success('Bölge başarıyla eklendi!'); // Toast bildirim
                    setTimeout(function () {
                        window.location.reload();  // Sayfayı yenile
                    }, 1500);
                } else {
                    toastr.error(response.message || 'Bölge eklenirken bir hata oluştu.'); // Toast bildirim
                }
            },
            error: function (xhr, status, error) {
                console.error('Bölge eklenirken bir hata oluştu:', error);
                toastr.error('Bir hata oluştu. Lütfen tekrar deneyin.'); // Toast bildirim
            }
        });
    });
});


$(document).ready(function () {
    // Bölgeleri Düzenle butonuna tıklama olayını yakalayın
    $('.btn-editArea').click(function () {
        // Modalı aç
        $('#listAreaModal').modal('show');

        // AJAX ile mevcut bölgeleri sunucudan çek
        $.ajax({
            url: '/Table/GetRegions', // Bölge verilerini çekeceğiniz controller action
            type: 'GET',
            success: function (regions) {
                // Bölge listesini temizle
                $('#regionList').empty();

                // Gelen bölgeleri listele
                regions.forEach(function (region) {
                    var listItem = `
                                <li class="list-group-item d-flex justify-content-between align-items-center">
                                    ${region.areaName}
                                    <a href="javascript:;" class="btn btn-edit-region" data-id="${region.tableAreaId}"><i class="fa-solid fa-pen"></i></a>
                                </li>
                            `;
                    $('#regionList').append(listItem);
                });

            },
            error: function (xhr, status, error) {
                console.error('Bölgeler yüklenirken hata oluştu:', error);
            }
        });
    });
});

$(document).ready(function () {
    // Bölge düzenleme butonuna tıklama olayını yakala
    $(document).on('click', '.btn-edit-region', function () {
        var regionId = $(this).data('id'); // Tıklanan bölgenin ID'sini al

        $('#listAreaModal').modal('hide');
        // AJAX ile bölge bilgilerini getir
        $.ajax({
            method: 'GET',
            url: '/Table/GetRegionById', // Controller'daki metodun URL'si
            data: { id: regionId }, // Gönderilecek veri
            success: function (region) {
                // Modal içeriğini güncelle
                $('#editRegionName').val(region.areaName);
                $('#updateAreaModal').data('id', regionId); // ID'yi modal veri olarak ayarla

                // Modalı göster
                $('#updateAreaModal').modal('show');
            },
            error: function (xhr, status, error) {
                console.error('Bölge bilgileri yüklenirken hata oluştu:', error);
            }
        });
    });
});

$(document).ready(function () {
    // Güncelleme butonuna tıklama olayını yakala
    $('#saveRegionBtn').on('click', function () {
        var regionId = $('#updateAreaModal').data('id'); // ID'yi modal veri olarak al
        var regionName = $('#editRegionName').val();

        // AJAX ile bölge adını güncelle
        $.ajax({
            method: 'POST',
            url: '/Table/UpdateRegion', // Controller'daki metodun URL'si
            contentType: 'application/json',
            data: JSON.stringify({
                areaId: regionId,
                areaName: regionName
            }),
            success: function (response) {
                console.log('Güncelleme başarılı:', response);

                // Modalı kapat
                $('#updateAreaModal').modal('hide');
                // Sayfayı güncelle veya bölgeler listesini yeniden yükle
                toastr.success('Bölge Başarıyla Güncellendi!'); // Toast bildirim
                setTimeout(function () {
                    window.location.reload();  // Sayfayı yenile
                }, 1500);
            },
            error: function (xhr, status, error) {
                toastr.error('Bölge güncellenirken hata oluştu:'); // Toast bildirim
                console.error('Hata yanıtı:', xhr.responseText); // Sunucudan dönen hata yanıtını kontrol et
            }
        });
    });
});

$(document).ready(function () {
    $('.btn-moveArea').click(function () {
        $('#sortRegionsModal').modal('show');

        // AJAX ile mevcut bölgeleri sunucudan çek
        $.ajax({
            url: '/Table/GetRegions', // Bölge verilerini çekeceğiniz controller action
            type: 'GET',
            success: function (regions) {
                // Bölgeleri listele
                var listItems = '';
                regions.forEach(function (region) {
                    listItems += `
                                <li class="list-group-item d-flex justify-content-between align-items-center" data-id="${region.tableAreaId}">
                                    ${region.areaName}
                                    <i class="fa-solid fa-arrow-up-down"></i>
                                </li>
                            `;
                });
                $('#sortableRegions').html(listItems);

                // Sortable özelliğini başlat
                $('#sortableRegions').sortable({
                    placeholder: 'ui-state-highlight',
                    update: function (event, ui) {
                        // Sıra değiştirildiğinde yapılacak işlemler
                        var order = $(this).sortable('toArray', { attribute: 'data-id' });
                        console.log(order); // Mevcut sıra konsola yazdırılır
                    }
                });
            },
            error: function (xhr, status, error) {
                toastr.error('Bölgeler yüklenirken hata oluştu. Lütfen tekrar deneyin.'); // Toast bildirim
            }
        });
    });

    $('#saveOrderBtn').on('click', function () {
        // Bölge ID'lerini sıraya göre topla
        var orderedIds = [];
        $('#sortableRegions li').each(function () {
            var regionId = $(this).data('id');
            orderedIds.push(regionId);
        });

        console.log(orderedIds); // Bu satır ekleyin: Gönderilen veriyi kontrol edin

        // AJAX ile sunucuya gönder
        $.ajax({
            url: '/Table/UpdateRegionOrder',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(orderedIds),
            success: function (response) {
                toastr.success('Sıralama Başarıyla Güncellendi!'); // Toast bildirim
                setTimeout(function () {
                    window.location.reload();  // Sayfayı yenile
                }, 1500);
            },
            error: function (xhr, status, error) {
                toastr.error("Sıralama güncellenirken hata oluştu");
            }
        });
    });

});

$(document).ready(function () {
    var selectedAreaId = null;

    // Toplu Masa Ekleme butonuna tıklandığında modalı aç
    $('.btn-bulkAddTable').click(function () {
        $('#bulkAddTableModal').modal('show');

        // AJAX ile bölgeleri yükleyin
        $.ajax({
            url: '/Table/GetRegions', // Bölge verilerini çekeceğiniz controller action
            type: 'GET',
            success: function (regions) {
                // Bölgeleri select kutusuna ekleyin
                var options = '<option value="">Bolge Seciniz</option>';
                regions.forEach(function (region) {
                    options += `<option value="${region.tableAreaId}">${region.areaName}</option>`;
                });
                $('#areaSelect').html(options);
            },
            error: function (xhr, status, error) {
                toastr.error('Bölgeler yüklenirken hata oluştu.'); // Toast bildirim
            }
        });
    });

    // Kaydet butonuna tıklandığında yapılacak işlemler
    $('#saveBulkTables').click(function () {
        var tableName = $('#tableName').val();
        var tableCount = parseInt($('#tableCount').val());
        var tableShape = $('input[name="tableShape"]:checked').val();
        var tableAreaId = $('#areaSelect').val(); // Seçili bölge ID'sini alın

        // Seçili bölge ID'sinin olup olmadığını kontrol et
        if (!tableAreaId) {
            toastr.error('Lütfen Bir Bölge Seçiniz!');
            return;
        }

        // Form verilerini AJAX ile sunucuya gönder
        $.ajax({
            url: '/Table/BulkAddTables', // Sunucu tarafındaki endpoint
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                tableName: tableName,
                tableCount: tableCount,
                tableShape: tableShape,
                tableAreaId: tableAreaId // Seçili bölge ID'sini gönder
            }),
            success: function (response) {
                toastr.success('Masalar başarıyla eklendi.'); // Toast bildirim
                $('#bulkAddTableModal').modal('hide');
                setTimeout(function () {
                    window.location.reload();  // Sayfayı yenile
                }, 1500);
            },
            error: function (xhr, status, error) {
                toastr.error('Masalar eklenirken hata oluştu:', error); // Toast bildirim
            }
        });
    });
});

$(document).ready(function () {
    var selectedTableId = null; // Seçilen masa ID'sini saklamak için

    // Masa düzenleme butonuna tıklandığında modalı aç
    $('.drop-area').on('click', '.btn-editTable', function () {
        selectedTableId = $(this).closest('.draggable-box').data('id'); // Seçili masa ID'sini sakla

        // Seçili masa bilgilerini AJAX ile al
        $.ajax({
            method: 'GET',
            url: '/Table/GetTableById', // Controller'daki metodun URL'si
            data: { tableId: selectedTableId },
            success: function (table) {
                // Modal içeriğini güncelle
                $('#editTableName').val(table.tableName);
                if (table.shape === 'Kare') {
                    $('#editShapeSquare').prop('checked', true);
                } else if (table.shape === 'Yuvarlak') {
                    $('#editShapeRound').prop('checked', true);
                }

                // Modal'ı göster
                $('#editTableModal').modal('show');
            },
            error: function (xhr, status, error) {
                toastr.error('Masa bilgileri alınırken hata oluştu:'); // Toast bildirim
            }
        });
    });

    // Kaydet butonuna tıklandığında yapılacak işlemler
    $('#saveTableChanges').click(function () {
        if (selectedTableId === null) {
            toastr.error('Lütfen bir masa seçin.');
            return;
        }

        var tableName = $('#editTableName').val();
        var tableShape = $('input[name="editTableShape"]:checked').val();

        // Form verilerini AJAX ile sunucuya gönder
        $.ajax({
            method: 'POST',
            url: '/Table/Update', // Controller'daki metodun URL'si
            contentType: 'application/json',
            data: JSON.stringify({
                tableId: selectedTableId,
                tableName: tableName,
                tableShape: tableShape
            }),
            success: function (response) {
                toastr.success('Masa başarıyla güncellendi!'); // Toast bildirim
                $('#editTableModal').modal('hide');
                setTimeout(function () {
                    window.location.reload();  // Sayfayı yenile
                }, 1500);
            },
            error: function (xhr, status, error) {
                toastr.error('Masa güncellenirken hata oluştu.')
            }
        });
    });
});

toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};