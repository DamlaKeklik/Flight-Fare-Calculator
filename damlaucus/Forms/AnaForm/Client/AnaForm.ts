interface Sehir {
	result: {
		DGID: number,
		Sehirler: string,
		ENLEM: number,
		BOYLAM: number,
	}[]
	
}
interface Baga {
	result: {
		BAGAJ: number,
		MILUCRET: number,
		NumberBox1: number,
	}[]
	
}

export default class AnaForm extends Form.Designer {

	async VARIS_OnSelectedItemChanging(args: Controls.EventArgs.IPropertyChangingEventArgs<Controls.Common.IListItem<Controls.Common.MultiLanguageText>>) {
                console.log(args);
		if(args.newValue.value==this.KALKIS.value){
			this.showMessage("HATA","Aynı şehri varış ve kalkışta seçemezsin!","Error")
			args.cancel=true
		}
	}

    async TOPLAMG_OnValueChanging(args: Controls.EventArgs.IPropertyChangingEventArgs<number>) {
        console.log(args);
        
    }

    async BAGAJG_OnValueChanging(args: Controls.EventArgs.IPropertyChangingEventArgs<number>) {
        console.log(args);

        try {
            var sehirlerResult = await this.fetch.post<Sehir>("DataSource/sehirler");
            var ucretlerResult = await this.fetch.post<Baga>("DataSource/standartucretler");
            console.log(ucretlerResult);
            var kalkis = this.KALKIS.value;
            var varis = this.VARIS.value;
            var bagajAgirligi = args.newValue; // Değişen bagaj ağırlığı

            if (!sehirlerResult || !sehirlerResult.data) {
                console.log(sehirlerResult)
                console.error("Şehirler verisi beklenmedik bir yapıda.");
                return;
            }

            if (!ucretlerResult || !ucretlerResult.data) {
                console.error("Ücretler verisi beklenmedik bir yapıda.");
                return;
            }

            let kalkisKoordinat = null;
            let varisKoordinat = null;

            
            sehirlerResult.data.result.forEach(sehir => {
                if (sehir.DGID == kalkis) {
                    kalkisKoordinat = { lat: sehir.ENLEM, lon: sehir.BOYLAM };
                }
                if (sehir.DGID == varis) {
                    varisKoordinat = { lat: sehir.ENLEM, lon: sehir.BOYLAM };
                }
            });

            let mesafe = 0;
            if (kalkisKoordinat && varisKoordinat) {
                // Koordinatlar bulundu, mesafe hesapla
                mesafe = this.koordinatHesapla(kalkisKoordinat, varisKoordinat);
                console.log("Mesafe: " + mesafe + " mil");
            } else {
                console.error("Kalkış veya varış koordinatları bulunamadı.");
                return;
            }

            
            let bagajLimiti = null;
            let kiloBasiUcret = null;
            let milUcreti = null;

            ucretlerResult.data.result.forEach(ucret => {
                // Tek bir satırdan değerleri alıyoruz
                 bagajLimiti = ucret.BAGAJ;
                 kiloBasiUcret = ucret.NumberBox1; // Aşılan kilo başına ücret
                 milUcreti = ucret.MILUCRET;
             });

            let asimUcreti = 0;
            if (bagajLimiti !== null && kiloBasiUcret !== null) {
                if (bagajAgirligi > bagajLimiti) {
                    let asimMiktari = bagajAgirligi - bagajLimiti;
                    asimUcreti = asimMiktari * kiloBasiUcret;
                    console.log("Aşım miktarı: " + asimMiktari + " kg");
                    console.log("Aşım ücreti: " + asimUcreti + " TL");
                } else {
                    console.log("Bagaj ağırlığı limit içinde.");
                }
            } else {
                console.error("Bagaj limiti veya kilo başı ücret bulunamadı.");
            }

            let toplamMilUcreti = 0;
            if (milUcreti !== null) {
                toplamMilUcreti = mesafe * milUcreti; // Mesafeye göre mil ücreti hesapla
                console.log("Mil ücreti: " + toplamMilUcreti + " TL");
            } else {
                console.error("Mil ücreti bulunamadı.");
                return;
            }

            
            const totalAmount = toplamMilUcreti + asimUcreti;
            this.TOPLAMG.value = totalAmount

        } catch (error) {
            console.error("Veri çekme işlemi sırasında hata oluştu: ", error);
        }
    }

    koordinatHesapla(koordinat1, koordinat2) {
        // Haversine formülü ile iki nokta arasındaki mesafeyi hesaplama
        const R = 3958.8; // Dünya'nın yarıçapı (mil)
        const dLat = this.deg2rad(koordinat2.lat - koordinat1.lat);
        const dLon = this.deg2rad(koordinat2.lon - koordinat1.lon);
        const a = 
            Math.sin(dLat/2) * Math.sin(dLat/2) +
            Math.cos(this.deg2rad(koordinat1.lat)) * Math.cos(this.deg2rad(koordinat2.lat)) * 
            Math.sin(dLon/2) * Math.sin(dLon/2);
        const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
        const mesafe = R * c; // Mesafe (mil)
        return mesafe;
    }

    deg2rad(deg) {
        return deg * (Math.PI/180);
    }


	async KALKIS_OnSelectedItemChanging(args: Controls.EventArgs.IPropertyChangingEventArgs<Controls.Common.IListItem<Controls.Common.MultiLanguageText>>) {
        console.log(args);
		if(args.newValue.value==this.VARIS.value){
			this.showMessage("HATA","Aynı şehri varış ve kalkışta seçemezsin!","Error")
			args.cancel=true
		}
	}

}