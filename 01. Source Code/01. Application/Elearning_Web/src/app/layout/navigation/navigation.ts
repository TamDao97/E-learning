import { NtsNavigation } from './navigationitem';

export const navigation: NtsNavigation[] = [
    {
        id: 'caidat',
        title: 'Dashboard',
        icon: 'home',
        type: 'item',
        ismdi: true,
        permission: ['F000000'],
        url: '/dash-board/dash-board'
    },
    {
        id: 'chuongtrinh',
        title: 'Đào tạo',
        icon: 'school',
        type: 'collapsable',
        ismdi: true,
        permission: ['F000050','F000100','F000200','F000250'],
        children: [
            {
                id: 'chuongtrinhdaotao',
                title: 'Chương trình đào tạo',
                type: 'item',
                permission: ['F000050'],
                url: '/dao-tao/chuong-trinh-dao-tao'
            },
            {
                id: 'khoahoc',
                title: 'Khóa học',
                type: 'item',
                permission: ['F000100'],
                url: '/dao-tao/khoa-hoc'
            },
            {
                id: 'quanlytiendo',
                title: 'Quản lý tiến độ',
                type: 'item',
                permission: ['F000200'],
                url: '/dao-tao/quan-ly-tien-do'
            },
            {
                id: 'quanlybinhluan',
                title: 'Quản lý bình luận',
                type: 'item',
                permission: ['F000250'],
                url: '/dao-tao/quan-ly-binh-luan'
            },
        ]
    },
    {
        id: 'dulieubaigiang',
        title: 'Dữ liệu bài giảng',
        icon: 'school',
        type: 'collapsable',
        ismdi: true,
        permission: ['F000300','F000400'],
        children: [
            {
                id: 'quanlybaigiang',
                title: 'Quản lý bài giảng',
                type: 'item',
                permission: ['F000300'],
                url: '/dao-tao/quan-ly-bai-giang'
            },
            {
                id: 'nganhangcauhoi',
                title: 'Ngân hàng câu hỏi',
                type: 'item',
                permission: ['F000400'],
                url: '/questions/questions-manager',
            },
        ]
    },
    {
        id: 'thongke',
        title: 'Thống kê',
        icon: 'mdi mdi-chart-bar',
        type: 'collapsable',
        ismdi: true,
        permission: ['F000550','F000600','F000650','F000700'],
        children: [
            {
                id: 'thongkenguoihoc',
                title: 'Thống kê người đăng ký học',
                type: 'item',
                permission: ['F000550'],
                url: '/report/thong-ke-nguoi-hoc'
            },
            {
                id: 'thongkethongtinnguoihoc',
                title: 'Thống kê thông tin người học',
                type: 'item',
                permission: ['F000600'],
                url: '/report/thong-ke-thong-tin-nguoi-hoc'
            },
            {
                id: 'ketquahoanthanh',
                title: 'Kết quả hoàn thành khóa học',
                type: 'item',
                permission: ['F000650'],
                url: '/report/thong-ke-so-luong-hoan-thanh-khoa-hoc'
            },
            {
                id: 'ketquahoanthanh',
                title: 'Số lượng người đăng ký khóa học',
                type: 'item',
                permission: ['F000700'],
                url: '/report/thong-ke-so-dang-ky-khoa-hoc'
            },
        ]
    },
    {
        id: 'nguoidung',
        title: 'Tài khoản',
        icon: 'account',
        type: 'collapsable',
        ismdi: true,
        permission: ['F000350','F000450','F000500','F000150'],
        children: [
            {
                id: 'nhanvien',
                title: 'Quản trị hệ thống',
                type: 'item',
                permission: ['F000350'],
                url: '/nguoi-dung/quan-tri-he-thong'
            },
            {
                id: 'chuyengia',
                title: 'Quản lý chuyên gia',
                type: 'item',
                permission: ['F000450'],
                url: '/nguoi-dung/chuyen-gia'
            },
            {
                id: 'nguoihuongdan',
                title: 'Quản lý giảng viên',
                type: 'item',
                permission: ['F000500'],
                url: '/nguoi-dung/nguoi-huong-dan'
            },
            {
                id: 'nguoidung',
                title: 'Quản lý người học',
                type: 'item',
                permission: ['F000150'],
                url: '/nguoi-dung/nguoi-dung'
            },
            {
                id: 'quanlydonvichuquan',
                title: 'Cấu hình nhóm quyền',
                type: 'item',
                //permission: ['F000900'],
                url: '/setting/cau-hinh-nhom-quyen'
            },
        ]
    },
    {
        id: 'lichsu',
        title: 'Lịch sử',
        icon: 'account',
        type: 'collapsable',
        ismdi: true,
        permission: [],
        children: [
            {
                id: 'backend',
                title: 'Quản trị',
                type: 'item',
                permission: [],
                url: '/lich-su/quan-tri'
            },
            {
                id: 'fontend',
                title: 'Người học',
                type: 'item',
                permission: [],
                url: '/lich-su/nguoi-hoc'
            },
        ]
    },
    
    {
        id: 'caidat',
        title: 'Cấu hình',
        icon: 'cog',
        type: 'collapsable',
        ismdi: true,
        permission: ['F000750','F000800','F000850','F000900'],
        children: [
            {
                id: 'thietlapgiaodien',
                title: 'Thiết lập trang chủ',
                type: 'item',
                permission: ['F000750'],
                url: '/setting/cai-dat-trang-chu'
            },
            {
                id: 'quanlyloitua',
                title: 'Quản lý lời tựa',
                type: 'item',
                permission: ['F000800'],
                url: '/setting/quan-ly-loi-tua'
            },
            {
                id: 'sildebar',
                title: 'Silde bar',
                type: 'item',
                permission: ['F000850'],
                url: '/silde-bar'
            },
            {
                id: 'cauhinhchuyengia',
                title: 'Cấu hình chuyên gia',
                type: 'item',
                permission: ['F000900'],
                url: '/dao-tao/cau-hinh-chuyen-gia'
            },
            {
                id: 'quanlytranglienket',
                title: 'Quản lý trang liên kết',
                type: 'item',
                permission: ['F000900'],
                url: '/setting/quan-ly-trang-lien-ket'
            },
            {
                id: 'quanlydonvichuquan',
                title: 'Quản lý đơn vị chủ quản',
                type: 'item',
                //permission: ['F000900'],
                url: '/setting/quan-ly-don-vi-chu-quan'
            },
            {
                id: 'cauhinhthongsohethong',
                title: 'Cấu hình thông số',
                type: 'item',
                permission: ['F001100'],
                url: '/setting/cau-hinh-thong-so-he-thong'
            },
        ]
    },
   
    {
        id: 'template',
        title: 'Biểu mẫu',
        icon: 'file-document-multiple',
        type: 'collapsable',
        ismdi: true,
        permission: ['F000950','F001000'],
        children: [
            {
                id: 'chungchi',
                title: 'Mẫu chứng chỉ',
                type: 'item',
                permission: ['F000950'],
                url: 'bieu-mau/mau-chung-chi'
            },
            {
                id: 'mauhethong',
                title: 'Mẫu văn bản',
                type: 'item',
                permission: ['F001000'],
                url: 'bieu-mau/mau-van-ban'
            }
        ]
    },
    {
        id: 'about',
        title: 'Giới thiệu',
        icon: 'information',
        type: 'item',
        ismdi: true,
        permission: ['F001050'],
        url: '/page/gioi-thieu'
    },
];