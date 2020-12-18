using UnityEngine;

public class KeyWord
{
    public static string ID = "id";
    public static string NAME = "player.name";
    public static string USER_INSTANCE= "user.instance";

    public static string GAME_MANAGER = "game.manager";
    public static string AUDIO_MANAGER = "audio.manager";
    public static string MAINMENU = "Main Menu";
    public static string GAME_LOGIN = "Game Login";
    public static string MAIN_CANVAS = "main.canvas";
    public static string COMPLETE_TASK_MANAGER = "complete.task.manager";
    public static string INFO_MANAGER = "info.manager";
    public static string INFO_PANEL = "info.panel";
    public static string CHAT = "chat";
    public static string LOADING_PAGE = "loading.page";
    public static string GAME_BOARD = "game.board";

    public static string RED_COLOR_TAG = "<color=red>";
    public static string GREEN_COLOR_TAG = "<color=green>";
    public static string YELLOW_COLOR_TAG = "<color=yellow>";
    public static string MAGENTA_COLOR_TAG = "<color=#ff00ffff>";
    public static string WHITE_COLOR_TAG = "<color=#ffffffff>";
    public static string GREENFIELD_COLOR_TAG = "<color=#00C835>";
    public static string REDCRIMSON_COLOR_TAG = " <color=#CD252F>";
    public static string CLOSE_COLOR_TAG = "</color>";
    public static string BIG_TAG = "<b>";
    public static string CLOSE_ALL_TAG = "</b></color>";

    public static string GAME1_INSTRUCTION = "- Kumpulkan bahan-bahan yang diperlukan" +
                " untuk mendapatkan poin. Yang pertama mendapatkan" +
                 BIG_TAG + GREENFIELD_COLOR_TAG + " 9 Poin" + CLOSE_ALL_TAG +
                " ialah pemenangnya!" + REDCRIMSON_COLOR_TAG + " (Jika" +
                " mengambil bahan yang sudah diambil musuh, maka poin" +
                " tidak dihitung!)" + CLOSE_COLOR_TAG;

    public static string GAME2_INSTUCTION = "- Selesaikan bumbu-bumbu yang diperlukan" +
                " untuk mendapaikan poin. Yang pertama mendapatkan" +
                BIG_TAG + GREENFIELD_COLOR_TAG + " 2 Poin" + CLOSE_ALL_TAG +
                " ialah pemenangnya! " + RED_COLOR_TAG+ "(Tekan tombol musik untuk mendengarkan resep tiap bumbu)" + CLOSE_COLOR_TAG;

    public static string GAME3_INSTRUCTION = "- Buat kuah dan sajikan kupat glabed" +
                " untuk mendapatkan poin. Yang pertama mendapatkan" +
                BIG_TAG + GREENFIELD_COLOR_TAG + " 2 Poin" + CLOSE_ALL_TAG +
                " ialah pemenangnya!"+RED_COLOR_TAG+
                " (Gunakan Penggorengan untuk membuat kuah dan Piring untuk menata kupat glabed)" + CLOSE_COLOR_TAG;

    public static Color GreenField = new Color(0f/255f, 229f/255f, 60f/255f);
    public static Color RedCrimson = new Color(205f/255f, 0f/255f, 0f/255f);
    public static Color BlueGrey = new Color(100f/255f, 140f/255f, 168f/255f);

    //  GAME 1
    public static string GAME1 = "Game 1";
    public static string GAME1_MANAGER = "game1.manager";
    public static string GAME1_READY = "game1.ready";
    public static string GAME1_DONE = "game1.doned";

    public static string GAME1_BAWANG_BOMBAI = "bawang.bombai";
    public static string GAME1_BAWANG_PUTIH = "bawang.putih";
    public static string GAME1_KEMIRI = "kemiri";
    public static string GAME1_KENCUR = "kencur";
    public static string GAME1_KUNIR = "kunir";
    public static string GAME1_LAOS = "laos";
    public static string GAME1_SERAI = "serai";
    public static string GAME1_KETUMBAR = "ketumbar";
    public static string GAME1_CABAI = "cabai";
    public static string GAME1_BAWANG_MERAH = "bawang.merah";
    public static string GAME1_TOMAT_MERAH = "tomat.merah";
    public static string GAME1_GULA = "gula";
    public static string GAME1_GARAM = "garam";
    public static string GAME1_TEMPTE = "tempe";
    public static string GAME1_AIR = "air";
    public static string GAME1_SANTAN = "santan";
    public static string GAME1_KETUPAT = "ketupat";

    public static string GAME1_MATERIAL_DONE = "material.done";
    public static string GAME1_PLAYER_MATERIAL = "game1.player.material";
    public static string GAME1_ENEMY_MATERIAL = "game1.enemy.material";

    // GAME 2
    public static string GAME2 = "Game 2";
    public static string GAME2_MANAGER = "game2.manager";
    public static string GAME2_READY = "game2.ready";
    public static string GAME2_DONE = "game2.doned";

    public static string GAME2_BUMBU_KUNING = "bumbu.kuning";
    public static string GAME2_BUMBU_MERAH = "bumbu.merah";
    public static string GAME2_ANIM_PROP_BLENDER_GAGAL = "Animasi Blender Gagal";
    public static string GAME2_ANIM_PROP_BLENDER_BUMBU_KUNING = "Animasi Blender Bumbu Kuning";
    public static string GAME2_ANIM_PROP_BLENDER_BUMBU_MERAH = "Animasi Blender Bumbu Merah";

    public static string GAME2_BUMBU_DONE = "game2.bumbu.done";

    // GAME 3 
    public static string GAME3 = "Game 3";
    public static string GAME3_MANAGER = "game3.manager";
    public static string GAME3_READY = "game3.ready";
    public static string GAME3_DONE = "game3.doned";

    public static string GAME3_HASIL_KUAH = "hasil.kuah";
    public static string GAME3_KUAH_DONE = "kuah.doned";
    public static string GAME3_KUAH_STEP1 = "STEP 1: Masukkan Air";
    public static string GAME3_KUAH_STEP2 = "STEP 2: Masukkan Bumbu Kuning";
    public static string GAME3_KUAH_STEP3 = "STEP 3: Klik Penggorengan 3x";
    public static string GAME3_KUAH_STEP4 = "STEP 4: Masukkan Tomat";
    public static string GAME3_KUAH_STEP5 = "STEP 5: Masukkan Garam";
    public static string GAME3_KUAH_STEP6 = "STEP 6: Masukkan Gula";
    public static string GAME3_KUAH_STEP7 = "STEP 7: Masukkan Bumbu Merah";
    public static string GAME3_KUAH_STEP8 = "STEP 8: Klik Penggorengan 3x";

    public static string GAME3_KUPAT_GLABED = "kupat.glebed";
    public static string GAME3_KUPAT_GLABED_DONE = "kupat.glabed.doned";
    public static string GAME3_KUPAT_GLABED_STEP1 = "STEP 1: Masukkan Ketupat";
    public static string GAME3_KUPAT_GLABED_STEP2 = "STEP 2: Masukkan Tempe";
    public static string GAME3_KUPAT_GLABED_STEP3 = "STEP 3: Masukkan Kuah";
    public static string GAME3_KUPAT_GLABED_STEP4 = "STEP 4: Klik Mangkok 3x";
    public static string GAME3_KUPAT_GLABED_STEP5 = "STEP 5: Klik Mangkok 1x";

    public static string GAME3_ANIMASI_KUAH_STEP0 = "kuah step0";
    public static string GAME3_ANIMASI_KUAH_STEP1 = "kuah step1";
    public static string GAME3_ANIMASI_KUAH_STEP2 = "kuah step2";
    public static string GAME3_ANIMASI_KUAH_STEP3 = "kuah step3";
    public static string GAME3_ANIMASI_KUAH_STEP7 = "kuah step7";
    public static string GAME3_ANIMASI_KUAH_STEP8 = "kuah step8";

    public static string GAME3_ANIMASI_KUPAT_STEP0 = "kupat step0";
    public static string GAME3_ANIMASI_KUPAT_STEP1 = "kupat step1";
    public static string GAME3_ANIMASI_KUPAT_STEP3 = "kupat step3";
    public static string GAME3_ANIMASI_KUPAT_STEP4 = "kupat step4";
    public static string GAME3_ANIMASI_KUPAT_STEP5 = "kupat step5";

    public static string ANIMASI_CLIP_KUAH_3B = "Meja_Kuah3b";
    public static string ANIMASI_CLIP_KUAH_4B = "Meja_Kuah4b";
    public static string ANIMASI_CLIP_KUPAT_3B = "Meja_Kupat3b";
}

public class Key_Data
{
    public static string USER = "users";
    public static string USER_ID = "userID";
    public static string NICKNAME = "nickname";
    public static string SCORE = "score";
    public static string LEADERBOARD = "leaderboard";
}
