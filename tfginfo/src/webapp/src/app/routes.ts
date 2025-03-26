export const route = {
    admin: {
        department: {
            create: '/admin/department/create',
            edit: '/admin/department/edit',
            list: '/admin/department',
        },
        university: {
            create: '/admin/university/create',
            edit: '/admin/university/edit',
            list: '/admin/university',
        },
        user: {
            create: '/admin/user/create',
            edit: '/admin/user/edit',
            list: '/admin/user',
        }
    },
    booking: '/',
    login: '/login',
    professor: {
        create: '/professor/create',
        edit: '/professor/edit',
        list: '/professor',
    },
    profile: '/profile',
    studingGroup: {
        create: '/studing-group/create',
        edit: '/studing-group/edit',
        list: '/studing-groups',
    },
    tfg: {
        create: '/tfg/create',
        edit: '/tfg/edit',
        list: '/tfg',
    }
}